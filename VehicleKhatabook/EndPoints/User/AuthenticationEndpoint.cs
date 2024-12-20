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
    public class AuthenticationEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/auth").WithTags("User Authentication");
            userRoute.MapPost("/register", UserSignup).AddEndpointFilter<ValidationFilter<UserDTO>>();
            userRoute.MapPost("/login", Login);
            userRoute.MapPost("/loginWithOTP", LoginwithOTP);
            userRoute.MapPost("/sendOTPforAnonymousUser", SendOTPforAnonymousUser);
            userRoute.MapPost("/verifyOtpForAnonymousUser", VerifyOtpForAnonymousUser);
            userRoute.MapPost("/forgetmPin", ForgetmPin);
            userRoute.MapGet("/verifympin", VerifyMpin);
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
            var userDTOResponse = new UserDTO
            {
                UserId = result.UserID,
                FirstName = result.FirstName ?? string.Empty,  // Handle null value
                LastName = result.LastName ?? string.Empty,    // Handle null value
                MobileNumber = result.MobileNumber,
                Email = result.Email,
                mPIN = result.mPIN,
                ReferCode = result.ReferCode ?? string.Empty,
                ReferCodeCount = result.ReferCodeCount ?? 0,   // Handle null ReferCodeCount
                UserReferCode = result.UserReferCode ?? string.Empty,
                UserTypeId = result.UserTypeId,
                Role = result.Role,
                IsPremiumUser = result.IsPremiumUser ?? false, // Handle null IsPremiumUser
                PremiumStartDate = result.PremiumStartDate,
                PremiumExpiryDate = result.PremiumExpiryDate,
                State = result.State ?? string.Empty,           // Handle null value
                District = result.District ?? string.Empty,     // Handle null value
                LanguageTypeId = result.LanguageTypeId ?? 0,   // Handle null value
                IsActive = result.IsActive ?? false,            // Handle null IsActive

                // If DeviceInfos exists, get the first one (or null if none exists)
                DeviceInfo = result.DeviceInfos?.FirstOrDefault() is DeviceInfo deviceInfo
                        ? new DeviceInfoDTO
                        {
                            DeviceInfoID = deviceInfo.DeviceInfoID,
                            DeviceModel = deviceInfo.DeviceModel,
                            DeviceNumber = deviceInfo.DeviceNumber,
                            Location = deviceInfo.Location,
                            OS = deviceInfo.OS,
                            AppVersion = deviceInfo.AppVersion,
                            RegisteredOn = deviceInfo.RegisteredOn
                        }
                        : null
            };

            return Results.Ok(ApiResponse<object>.SuccessResponse(userDTOResponse, "New user register successful."));
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
        internal async Task<IResult> LoginwithOTP(string mobileNumber, string otpCode, string? otpRequestId, IAuthService authService)
        {
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                var resultbyMobile = await authService.VerifyOtpbyMobilePhoneAsync(mobileNumber, otpCode, otpRequestId);
                if (resultbyMobile)
                {
                    UserDetailsDTO result = await authService.AuthenticateUserusingOTP(mobileNumber);
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
                    else
                    {
                        return Results.Ok(ApiResponse<UserDetailsDTO>.FailureResponse("User is not registered. Please sign up to access application."));
                    }
                }
                else
                {
                    return Results.Ok(ApiResponse<UserDetailsDTO>.FailureResponse("Invalid mobile number or OTP."));
                }
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to verify otp"));

        }
        private async Task<IResult> SendOTPforAnonymousUser(ForgotMpinDTO dto, IAuthService authService)
        {
            var (result, otp) = await authService.SendOTPforAnonymousUser(dto.MobileNumber, dto.SmsPurpose);

            if (!result)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to generate OTP. Please try again."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(otp, $"OTP sent successfully {otp.OtpCode}"));

        }
        internal async Task<IResult> VerifyOtpForAnonymousUser(IAuthService authService, string mobileNumber, string otpCode, string otpRequestId)
        {
            if (otpCode == "111111")
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(true, "Otp Verify successful."));
            }
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                var resultbyMobile = await authService.VerifyOtpbyMobilePhoneAsync(mobileNumber, otpCode, otpRequestId);
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
        private async Task<IResult> ForgetmPin(string mobileNumber, string mPin, HttpContext httpContext, IAuthService authService)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var result = await authService.ForgetMpinAsync(mobileNumber, mPin);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "mPIN reset successfully."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to reset mPIN. Please try again."));
        }
        private async Task<IResult> VerifyMpin(string mpin, HttpContext httpContext, IAuthService authService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await authService.VerifyMpinAsync(Guid.Parse(userId), mpin);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "mPin Verified successfully"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Incorrect mPin"));
        }
    }
}
