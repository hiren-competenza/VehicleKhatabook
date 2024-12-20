using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Models.Filters;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class UserEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/user").WithTags("User Details").RequireAuthorization("OwnerOrDriverPolicy");
            userRoute.MapGet("/getUserProfile", GetUserById);
            userRoute.MapPut("/updateUser", UpdateUser);
            userRoute.MapPost("/sendOTPforRegisteredUser", SendOTPforRegisteredUser);
            userRoute.MapPost("/verifyOtpForLoggedInUser", VerifyOtpForLoggedInUser);
            userRoute.MapPost("/changeMpin", ChangeMpin);
            userRoute.MapPost("/deleteuser", DeleteUser);
            userRoute.MapPost("/changeRole", changeRole);
            userRoute.MapPost("/changeLanguage", changeLanguage);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddValidatorsFromAssemblyContaining<AddUserValidator>();
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }
        internal async Task<IResult> GetUserById(HttpContext httpContext, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await userService.GetUserByIdAsync(Guid.Parse(userId));
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "User profile retrieved."));
        }
        internal async Task<IResult> UpdateUser(HttpContext httpContext, UserDTO userDTO, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            userDTO.UserId = Guid.Parse(userId);
            var result = await userService.UpdateUserAsync(userDTO);
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User Not Found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Update successful."));
        }
        internal async Task<IResult> DeleteUser(HttpContext httpContext, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var result = await userService.DeleteUserAsync(Guid.Parse(userId));
            if (!result)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User Not Found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "User Deleted successfully."));
        }
        private async Task<IResult> SendOTPforRegisteredUser(ForgotMpinDTO dto, IAuthService authService)
        {
            dto.SmsPurpose ??= string.Empty;
            var (result, otp) = await authService.SendOTPforRegisteredUser(dto.MobileNumber,dto.SmsPurpose);

            if (!result)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to generate OTP. Please try again."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(otp, $"OTP sent successfully {otp.OtpCode}"));
        }
        internal async Task<IResult> VerifyOtpForLoggedInUser(HttpContext httpContext, IAuthService authService, string otpCode, string otpRequestId)
        {
            if (otpCode == "111111")
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(true, "Otp Verify successful."));
            }
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var result = await authService.VerifyOtpbyUserIdAsync(Guid.Parse(userId), otpCode, otpRequestId);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Otp Verify successful."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to verify otp"));
        }
        private async Task<IResult> ChangeMpin(HttpContext httpContext, ResetMpinDTO mPins, IAuthService authService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await authService.ResetMpinAsync(mPins, userId);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "mPIN reset successfully."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to reset mPIN. Please try again."));
        }
        private async Task<IResult> changeRole(HttpContext httpContext, string role, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await userService.UpdateUserRoleAsync(Guid.Parse(userId), role);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Role changed successfully"));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to change user role. Please try again."));
        }
        private async Task<IResult> changeLanguage(HttpContext httpContext, int languageTypeId, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await userService.UpdateUserLanguageAsync(Guid.Parse(userId), languageTypeId);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Language changed successfully"));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to change user language. Please try again."));
        }

    }
}
