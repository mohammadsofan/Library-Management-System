using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Wrappers;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterUserRequestDto request);
        Task<ServiceResult<object>> LoginUserAsync(LoginUserRequestDto request);
        Task<ServiceResult> ChangeUserPasswordAsync(ChangeUserPasswordRequestDto request);
        Task<ServiceResult> ConfirmEmailAsync(ConfirmEmailRequestDto request);
        Task<ServiceResult<ICollection<ApplicationUserResponseDto>>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser, bool>> filter);
        Task<ServiceResult<ApplicationUserResponseDto>> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter);
    }
}
