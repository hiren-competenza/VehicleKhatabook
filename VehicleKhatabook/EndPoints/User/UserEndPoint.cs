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
            var userRoute = app.MapGroup("api/user").WithTags("User Details and Driver").RequireAuthorization("OwnerOrDriverPolicy");
            userRoute.MapGet("/GetUserProfile", GetUserById);
            userRoute.MapPut("/UpdateUser", UpdateUser);
            //userRoute.MapDelete("/{id:guid}", DeleteUser);
            //userRoute.MapGet("/", GetAllUsers);
            userRoute.MapGet("/getExpenseIncomeCategoriesById", GetExpenseIncomeCategoriesAsync);
            userRoute.MapGet("/GetDriverDetailsById", GetDriverDetailsById);
            userRoute.MapPut("/UpdateDriver", UpdateDriver);
            userRoute.MapDelete("/DeleteDriver", DeleteDriver);
            userRoute.MapGet("/GetAllDrivers", GetAllDrivers);
            userRoute.MapGet("/GetAllCountry", GetCountryAsync);
            userRoute.MapGet("/GetAllSMSProvider", GetAllSMSProviders);
            userRoute.MapPost("/VerifyOtpForLoggedInUser", VerifyOtpForLoggedInUser);
            userRoute.MapPost("/changeMPpin", ResetMpin);// Must be Authorized
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

        internal async Task<IResult> DeleteUser(Guid id, IUserService userService)
        {
            var result = await userService.DeleteUserAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        }

        internal async Task<IResult> GetAllUsers(IUserService userService)
        {
            var result = await userService.GetAllUsersAsync();
            return Results.Ok(result);
        }
       
        private async Task<IResult> GetExpenseIncomeCategoriesAsync(HttpContext httpContext, IUserService userService,IMasterDataService masterDataService, bool active = true)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var user = await userService.GetUserByIdAsync(Guid.Parse(userId));
            //GetUserByIdAsync
            if (user != null)
            {
                var incomeCategories = await masterDataService.GetIncomeCategoriesAsync(user.UserTypeId);
                var expenseCategories = await masterDataService.GetExpenseCategoriesAsync(user.UserTypeId);

                var response = new
                {
                    IncomeCategory = incomeCategories,
                    ExpenseCategory = expenseCategories
                };
                //var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
                return Results.Ok(ApiResponse<object>.SuccessResponse(response));
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User Not Found"));
            }
        }
        

        internal async Task<IResult> GetDriverDetailsById(HttpContext httpContext, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var driver = await userService.GetDriverByIdAsync(Guid.Parse(userId));
            if (driver == null)
                Results.Ok(ApiResponse<object>.FailureResponse("Driver not found"));

            return Results.Ok(ApiResponse<object>.SuccessResponse(driver, "Driver details found"));
        }

        internal async Task<IResult> UpdateDriver(HttpContext httpContext, UserDTO userDTO, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            if (userDTO == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid request body"));
            }
            userDTO.UserId = Guid.Parse(userId);
            var updateDriver = await userService.UpdateDriverAsync(Guid.Parse(userId), userDTO);
            if (updateDriver == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to update"));

            return Results.Ok(ApiResponse<object>.SuccessResponse(updateDriver, "driver update successful."));
        }

        internal async Task<IResult> DeleteDriver(HttpContext httpContext, IUserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await userService.DeleteDriverAsync(Guid.Parse(userId));
            if (!result)
            {
                return Results.Ok("driver not found/failed to delete");
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }

        internal async Task<IResult> GetAllDrivers(IUserService userService)
        {
            var drivers = await userService.GetAllDriversAsync();
            if (drivers == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("List of Driver not found."));

            return Results.Ok(ApiResponse<object>.SuccessResponse(drivers, "Drivers retrieved successfully."));
        }
        internal async Task<IResult> GetCountryAsync(IMasterDataService masterDataService)
        {
            var countries = await masterDataService.GetCountryAsync();
            if (countries.Count == 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("not found country list"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(countries));
        }
        internal async Task<IResult> GetAllSMSProviders(ISMSProviderService sMSProviderService)
        {
            var allProviders = await sMSProviderService.GetAllSMSProvidersAsync();

            var activeProvider = allProviders.FirstOrDefault(provider => provider.IsActive);

            var result = ApiResponse<List<SMSProviderDTO>>.SuccessResponse(activeProvider != null
                ? new List<SMSProviderDTO> { activeProvider }
                : new List<SMSProviderDTO>());

            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not found active provider"));
            }

            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
        internal async Task<IResult> VerifyOtpForLoggedInUser(HttpContext httpContext, IAuthService authService, string otpCode)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var result = await authService.VerifyOtpbyUserIdAsync(Guid.Parse(userId), otpCode);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Otp Verify successful."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to verify otp"));
        }
        private async Task<IResult> ResetMpin(HttpContext httpContext, ResetMpinDTO mPins, IAuthService authService)
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

            var result = await userService.UpdateUserRoleAsync(Guid.Parse(userId),role);
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
