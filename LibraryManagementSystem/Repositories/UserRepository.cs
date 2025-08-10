using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password)
        {
            return await _userManager.CheckPasswordAsync(applicationUser, password);
        }
        public async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser applicationUser, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(applicationUser, password,true);
        }
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user,string password)
        {
            user.CreatedAt = DateTime.Now;
            user.LastUpdatedAt = DateTime.Now;
            var result = await _userManager.CreateAsync(user,password);
            if(result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (user is null)
                return IdentityResult.Failed();
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.LastUpdatedAt = DateTime.UtcNow;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ICollection<ApplicationUser>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _userManager.Users.Where(filter).ToListAsync();
        }

        public async Task<ApplicationUser?> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _userManager.Users.FirstOrDefaultAsync(filter);
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, ApplicationUser user)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.Id==id);
            if (existingUser is null)
                return IdentityResult.Failed();
            existingUser.Gender = user.Gender;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.CityId = user.CityId;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.LastUpdatedAt = DateTime.UtcNow;
            return await _userManager.UpdateAsync(existingUser);
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId,string oldPassword,string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return IdentityResult.Failed(new IdentityError() { Code="NotFound",Description="User not found"});
            }
            if(oldPassword == newPassword)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "samePassword", Description = "New password cannot be the same as the old password" });

            }
            return await _userManager.ChangePasswordAsync(user,oldPassword, newPassword);
        }
        public async Task<string> GetUserEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "NotFound", Description = "User not found" });
            }
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<string> GeneratePasswordResetCodeAsync(ApplicationUser user)
        {
            var code = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            var passwordResetCode = new PasswordResetCode()
            {
                ApplicationUserId = user.Id,
                Code = code,
                ExpirationDate = DateTime.UtcNow.AddMinutes(10),
            };
            await _context.PasswordResetCodes.AddAsync(passwordResetCode);
            await _context.SaveChangesAsync();
            return code;

        }
        public async Task<IdentityResult> ConfirmPasswordResetByCodeAsync(string email, string code, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "NotFound", Description = "User not found" });
            }
            var passwordResetCode = await _context.PasswordResetCodes.FirstOrDefaultAsync(
                p => p.Code == code &&
                p.IsUsed == false &&
                p.ApplicationUserId == user.Id &&
                p.ExpirationDate > DateTime.UtcNow);
            if (passwordResetCode is null)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "InvalidCode", Description = "Invalid Code" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token,newPassword);
            if (result.Succeeded) {
                passwordResetCode.IsUsed = true;
                await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<string?> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
