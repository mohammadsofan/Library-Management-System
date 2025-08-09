using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Wrappers;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;

namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _emailSender = emailSender;
            _configuration = configuration;
        }
        public async Task<ServiceResult> RegisterUserAsync(RegisterUserRequestDto request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userRepository.CreateUserAsync(user, request.Password);
            if(result.Succeeded)
            {
                var confirmEmailToken = await _userRepository.GetUserEmailConfirmationTokenAsync(user);
                var encodedToken = WebUtility.UrlEncode(confirmEmailToken);
                var confirmationUrl = _linkGenerator.GetUriByAction(
                    httpContext:_httpContextAccessor.HttpContext!,
                    action:"ConfirmEmail",
                    controller:"User",
                    values:new { userId = user.Id, encodedToken }
                    );
                await _emailSender.SendEmailAsync(user.Email!, "Welcome to Sofan-Library!", $@"
                        <h2>Welcome Sofan-Library, {user.FirstName} {user.LastName}!</h2>
                        <p>We're excited to have you on board.</p>
                        <p>Your account has been successfully created. You can now log in and be a part of our library after confirming your email!</p>
                        <a href='{confirmationUrl}'>Confirm Email</a>
                        <p>If you have any questions, feel free to reach out to us at <a href='{_configuration["EmailSender:Email"]}'>{_configuration["EmailSender:Email"]}</a>.</p>
                        <br/>
                        <p>Happy Shopping!<br/>The Sofan-Library Team</p>
                        ");
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
            var role = await _userRepository.GetUserRoleAsync(user)??"User";
            string token = _tokenService.GetToken(user.Id,user.Email!,role);
            return ServiceResult<object>.Ok(new { token }, "login succeeded");

        }
        public async Task<ServiceResult> ChangeUserPasswordAsync(ChangeUserPasswordRequestDto request)
        {
            var result = await _userRepository.ChangePasswordAsync(request.UserId,request.OldPassword,request.NewPassword);
            if (!result.Succeeded) { 
                return ServiceResult.Fail("Change password failed", result.Errors.Select(e=>e.Description).ToList());
            }
            return ServiceResult.Ok("Password changed successfully!");
        }
        public async Task<ServiceResult> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var result = await _userRepository.ConfirmEmailAsync(request.UserId,request.Token);
            if (!result.Succeeded)
            {
                return ServiceResult.Fail("Confirmation failed", result.Errors.Select(e => e.Description).ToList());
            }
            return ServiceResult.Ok("Confirmation succeeded");
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
