using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Wrappers;
using Mapster;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository,ITokenService tokenService) {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<ServiceResult> RegisterUserAsync(RegisterUserRequestDto request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userRepository.CreateUserAsync(user, request.Password);
            if(result.Succeeded)
            {
                return ServiceResult.Ok("User registered successfully!");
            }
            return ServiceResult.Fail("Registration failed", result.Errors.Select(e=>e.Description).ToList());
        }
        public async Task<ServiceResult<object>> LoginUserAsync(LoginUserRequestDto request)
        {
            string invalidMessage = "Invalid email/username or password";
            var emailOrUsername = request.EmailOrUsername.ToLower();
            var user = await _userRepository.GetOneUserByFilterAsync(u=>
            emailOrUsername == u.UserName!.ToLower() || emailOrUsername == u.Email!.ToLower());
            if(user is null)
            {
                return ServiceResult<object>.Fail("Login failed", new List<string>() { invalidMessage });
            }
            var checkPassword = await _userRepository.CheckPasswordAsync(user, request.Password);
            if (!checkPassword)
            {
                return ServiceResult<object>.Fail("Login failed", new List<string>() { invalidMessage });
            }
            var role = await _userRepository.GetUserRole(user)??"User";
            string token = _tokenService.GetToken(user.Id,user.Email!,role);
            return ServiceResult<object>.Ok(new { token }, "login succeeded");

        }
        public async Task<ServiceResult<ICollection<ApplicationUserResponseDto>>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser,bool>> filter)
        {
            var users = await _userRepository.GetAllUsersByFilterAsync(filter);
            return ServiceResult<ICollection<ApplicationUserResponseDto>>.
                Ok(users.Adapt<ICollection<ApplicationUserResponseDto>>(), "Users retrieved successfully!");
        }
        public async Task<ServiceResult<ApplicationUserResponseDto>> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            var user = await _userRepository.GetOneUserByFilterAsync(filter);
            if(user is null)
            {
                return ServiceResult<ApplicationUserResponseDto>.Fail("User not found", new List<string>() { "User not found" });
            }
            return ServiceResult<ApplicationUserResponseDto>.
                Ok(user.Adapt<ApplicationUserResponseDto>(), "User retrieved successfully!");
        }

    }
}
