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
            try
            {
                return await _userManager.CheckPasswordAsync(applicationUser, password);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser applicationUser, string password)
        {
            try
            {
                return await _signInManager.CheckPasswordSignInAsync(applicationUser, password, true);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            try
            {
                user.CreatedAt = DateTime.Now;
                user.LastUpdatedAt = DateTime.Now;
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                    }
                    await _userManager.AddToRoleAsync(user, "User");
                }
                return result;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user is null)
                    return IdentityResult.Failed();
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
                user.LastUpdatedAt = DateTime.UtcNow;
                return await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public async Task<ICollection<ApplicationUser>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
        {
            try
            {
                IQueryable<ApplicationUser> query = _userManager.Users;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return await query.ToListAsync();
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public async Task<ApplicationUser?> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null");
            }
            try
            {
                return await _userManager.Users.FirstOrDefaultAsync(filter);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, ApplicationUser user)
        {
            try
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
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
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return IdentityResult.Failed(new IdentityError() { Code = "NotFound", Description = "User not found" });
                }
                if (oldPassword == newPassword)
                {
                    return IdentityResult.Failed(new IdentityError() { Code = "samePassword", Description = "New password cannot be the same as the old password" });
                }
                return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<string> GetUserEmailConfirmationTokenAsync(ApplicationUser user)
        {
            try
            {
                return await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return IdentityResult.Failed(new IdentityError() { Code = "NotFound", Description = "User not found" });
                }
                return await _userManager.ConfirmEmailAsync(user, token);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<string> GeneratePasswordResetCodeAsync(ApplicationUser user)
        {
            try
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
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<IdentityResult> ConfirmPasswordResetByCodeAsync(string email, string code, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
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
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    passwordResetCode.IsUsed = true;
                    await _context.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public async Task<string?> GetUserRoleAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.FirstOrDefault();
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
    }
}
