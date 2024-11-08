using FluentValidation;
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
    public class UserSignUp_LoginEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/user").WithTags("User Registration and Login");
            userRoute.MapPost("/v1/register", UserSignup).AddEndpointFilter<ValidationFilter<UserDTO>>();
            userRoute.MapPost("/Login", Login);
            userRoute.MapPost("/SendOTPforRegisteredUser", SendOTPforRegisteredUser);
            userRoute.MapPost("/SendOTPforAnonymousUser", SendOTPforAnonymousUser);
            userRoute.MapPost("/VerifyOtpForAnonymousUser", VerifyOtpForAnonymousUser);
            //userRoute.MapPost("/AddDriver", AddDriver);
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

        internal async Task<IResult> UserSignup(UserDTO userDTO, IUserService userService)
        {
            var result = await userService.CreateUserAsync(userDTO);
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to register"));
            }
            else if (String.IsNullOrEmpty(result.MobileNumber))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Mobile number already exists."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "New user register successful."));
        }
        internal async Task<IResult> Login(UserLoginDTO userLoginDTO, IAuthService authService)
        {
            UserDetailsDTO result = await authService.AuthenticateUser(userLoginDTO);
            if (result != null)
            {
                string token = authService.GenerateToken(result);

                var responseData = new
                {
                    Token = token,
                    UserDetails = result
                };

                return Results.Ok(ApiResponse<object>.SuccessResponse(responseData, "Login successful."));
            }
            return Results.Ok(ApiResponse<UserDetailsDTO>.FailureResponse("Invalid mobile number or mPIN."));
        }
        private async Task<IResult> SendOTPforRegisteredUser(ForgotMpinDTO dto, IAuthService authService)
        {
            var (result, otp) = await authService.SendOTPforRegisteredUser(dto.MobileNumber);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(otp, $"OTP sent successfully : {otp.OtpCode}"));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to send OTP. Please try again."));
        }

        private async Task<IResult> SendOTPforAnonymousUser(ForgotMpinDTO dto, IAuthService authService)
        {
            var (result, otp) = await authService.SendOTPforAnonymousUser(dto.MobileNumber);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(otp, $"OTP sent successfully : {otp.OtpCode}"));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to send OTP. Please try again."));
        }

        internal async Task<IResult> VerifyOtpForAnonymousUser(IAuthService authService, string mobileNumber, string otpCode)
        {
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                var resultbyMobile = await authService.VerifyOtpbyMobilePhoneAsync(mobileNumber, otpCode);
                if (resultbyMobile)
                {
                    return Results.Ok(ApiResponse<object>.SuccessResponse(resultbyMobile, "Otp Verify successful."));
                }
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to verify otp"));
        }
        internal async Task<IResult> AddDriver(HttpContext httpContext, UserDTO userDTO, IUserService userService)
        {
            //var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            //}
            //userDTO.UserId = Guid.Parse(userId);
            if (userDTO == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("Driver details are invalid"));

            var result = await userService.AddDriverAsync(userDTO);
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to register new driver"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "New driver added successful."));
        }
    }
}
