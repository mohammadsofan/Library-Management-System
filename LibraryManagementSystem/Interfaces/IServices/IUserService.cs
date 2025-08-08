using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Wrappers;

namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterUserRequestDto request);
        Task<ServiceResult<object>> LoginUserAsync(LoginUserRequestDto request);
    }
}
