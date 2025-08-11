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
using LibraryManagementSystem.Enums;

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
            try
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
                    var htmlMessage = $@"
                        <h2>Welcome Sofan-Library, {user.FirstName} {user.LastName}!</h2>
                        <p>We're excited to have you on board.</p>
                        <p>Your account has been successfully created. You can now log in and be a part of our library after confirming your email!</p>
                        <a href='{confirmationUrl}'>Confirm Email</a>
                        <p>If you have any questions, feel free to reach out to us at <a href='{_configuration["EmailSender:Email"]}'>{_configuration["EmailSender:Email"]}</a>.</p>
                        <br/>
                        <p>Happy Shopping!<br/>The Sofan-Library Team</p>
                        ";
                    await _emailSender.SendEmailAsync(user.Email!, "Welcome to Sofan-Library!", htmlMessage);
                    return ServiceResult.Ok("User registered successfully!");
                }
                return ServiceResult.Fail("Registration failed", result.Errors.Select(e=>e.Description).ToList());
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Registration failed", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult<object>> LoginUserAsync(LoginUserRequestDto request)
        {
            try
            {
                string invalidMessage = "Invalid email/username or password";
                var emailOrUsername = request.EmailOrUsername.ToLower();
                var user = await _userRepository.GetOneUserByFilterAsync(u=>
                emailOrUsername == u.UserName!.ToLower() || emailOrUsername == u.Email!.ToLower());
                if(user is null)
                {
                    return ServiceResult<object>.Fail("Login failed", new List<string>() { invalidMessage }, ServiceResultStatus.NotFound);
                }
                var checkResult = await _userRepository.CheckPasswordSignInAsync(user, request.Password);
                if (checkResult.IsLockedOut)
                {
                    return ServiceResult<object>.Fail("Login failed", new List<string>() { $"Your account is locked till {user.LockoutEnd}" }, ServiceResultStatus.ValidationError);
                }
                else if(checkResult.RequiresTwoFactor)
                {
                    return ServiceResult<object>.Fail("Login failed", new List<string>() { "Two-factor authentication required." }, ServiceResultStatus.ValidationError);
                }
                else if (checkResult.IsNotAllowed)
                {
                    return ServiceResult<object>.Fail("Login failed", new List<string>() { "Confirm your email before logging in." }, ServiceResultStatus.ValidationError);
                }
                else if (!checkResult.Succeeded)
                {
                    return ServiceResult<object>.Fail("Login failed", new List<string>() { invalidMessage }, ServiceResultStatus.NotFound);
                }
                var role = await _userRepository.GetUserRoleAsync(user)??"User";
                string token = _tokenService.GetToken(user.Id,user.Email!,role);
                return ServiceResult<object>.Ok(new { token }, "login succeeded");
            }
            catch (Exception ex)
            {
                return ServiceResult<object>.Fail("Login failed", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult> ChangeUserPasswordAsync(ChangeUserPasswordRequestDto request)
        {
            try
            {
                var result = await _userRepository.ChangePasswordAsync(request.UserId,request.OldPassword,request.NewPassword);
                if (!result.Succeeded) { 
                    return ServiceResult.Fail("Change password failed", result.Errors.Select(e=>e.Description).ToList(), ServiceResultStatus.ValidationError);
                }
                return ServiceResult.Ok("Password changed successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Change password failed", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            try
            {
                var result = await _userRepository.ConfirmEmailAsync(request.UserId,request.Token);
                if (!result.Succeeded)
                {
                    return ServiceResult.Fail("Confirmation failed", result.Errors.Select(e => e.Description).ToList(), ServiceResultStatus.ValidationError);
                }
                return ServiceResult.Ok("Confirmation succeeded");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Confirmation failed", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult> SendPasswordResetCodeAsync(SendPasswordResetCodeRequestDto request)
        {
            try
            {
                var user = await _userRepository.GetOneUserByFilterAsync(u => u.Email!.ToLower() == request.Email.ToLower());
                if (user?.Email is null)
                {
                    return ServiceResult.Fail(
                        "Failed to send reset password code",
                        new List<string> { "User not found or invalid/missing email" },
                        ServiceResultStatus.NotFound
                    );
                }
                var code = await _userRepository.GeneratePasswordResetCodeAsync(user);

                var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; max-width: 600px; margin: auto;'>
                    <h2 style='color: #333;'>Reset Your Password</h2>
                    <p>Hello,</p>
                    <p>We received a request to reset your password for your account associated with this email: <strong>{user.Email}</strong>.</p>
                    <p>Please use the verification code below to reset your password:</p>
                    <div style='background-color: #f5f5f5; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; letter-spacing: 2px; border-radius: 8px; color: #333; margin: 20px 0;'>
                        {code}
                    </div>
                    <p>This code will expire in <strong>10 minutes</strong>.</p>
                    <p>If you didn't request this, you can safely ignore this email.</p>
                    <hr style='margin: 30px 0;' />
                    <p style='font-size: 0.8em; color: #999;'>&copy; {DateTime.UtcNow.Year} Sofan-Library. All rights reserved.</p>
                </div>";

                await _emailSender.SendEmailAsync(user.Email, "Reset Password Verification Code", htmlMessage);
                return ServiceResult.Ok("Reset password verification code sent successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Failed to send reset password code", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult> ConfirmPasswordResetAsync(ConfirmPasswordResetRequestDto request)
        {
            try
            {
                var user = await _userRepository.GetOneUserByFilterAsync(u => u.Email!.ToLower() == request.Email.ToLower());
                if (user?.Email is null)
                {
                    return ServiceResult.Fail(
                        "Failed to send reset password code",
                        new List<string> { "User not found or invalid/missing email" },
                        ServiceResultStatus.NotFound
                    );
                }
                var result = await _userRepository.ConfirmPasswordResetByCodeAsync(user.Email, request.Code, request.NewPassword);
                if (result.Succeeded)
                {
                    return ServiceResult.Ok("Password has been reset successfully!");
                }
                else
                {
                    return ServiceResult.Fail("Failed to reset password", result.Errors.Select(e => e.Description).ToList(), ServiceResultStatus.ValidationError);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Failed to reset password", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult<ICollection<ApplicationUserResponseDto>>> GetAllUsersByFilterAsync(Expression<Func<ApplicationUser,bool>> filter)
        {
            try
            {
                var users = await _userRepository.GetAllUsersByFilterAsync(filter);
                return ServiceResult<ICollection<ApplicationUserResponseDto>>.
                    Ok(users.Adapt<ICollection<ApplicationUserResponseDto>>(), "Users retrieved successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult<ICollection<ApplicationUserResponseDto>>.Fail("Failed to retrieve users", new List<string> { ex.Message });
            }
        }
        public async Task<ServiceResult<ApplicationUserResponseDto>> GetOneUserByFilterAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            try
            {
                var user = await _userRepository.GetOneUserByFilterAsync(filter);
                if(user is null)
                {
                    return ServiceResult<ApplicationUserResponseDto>.Fail("User not found", new List<string>() { "User not found" }, ServiceResultStatus.NotFound);
                }
                return ServiceResult<ApplicationUserResponseDto>.
                    Ok(user.Adapt<ApplicationUserResponseDto>(), "User retrieved successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult<ApplicationUserResponseDto>.Fail("Failed to retrieve user", new List<string> { ex.Message });
            }
        }

    }
}
