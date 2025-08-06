using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Interfaces.IRepositrories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user,string passowrd);
        Task<IdentityResult> UpdateUserAsync(string id,ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<ICollection<ApplicationUser>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser,bool>> filter);
        Task<ApplicationUser?> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter);
        Task<bool> CheckPasswordAsync(ApplicationUser applicationUser,string password);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string oldPassword, string newPassowrd);

    }
}
