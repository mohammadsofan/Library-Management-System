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

        public UserRepository(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password)
        {
            return await _userManager.CheckPasswordAsync(applicationUser, password);
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
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
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
        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser,string oldPassword,string newPassowrd)
        {
            return await _userManager.ChangePasswordAsync(applicationUser,oldPassword,newPassowrd);
        }
    }
}
