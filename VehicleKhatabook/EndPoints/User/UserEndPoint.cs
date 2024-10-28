using FluentValidation;
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
            userRoute.MapGet("/GetUserProfile{id:guid}", GetUserById);
            userRoute.MapPut("/UpdateUser", UpdateUser);
            //userRoute.MapDelete("/{id:guid}", DeleteUser);
            //userRoute.MapGet("/", GetAllUsers);
            userRoute.MapPost("/auth/forgot-mpin", ForgotMpin);
            userRoute.MapPost("/auth/change-mpin", ResetMpin);
            userRoute.MapGet("/getExpenseIncomeCategoriesById", GetExpenseIncomeCategoriesAsync);
            userRoute.MapPost("/OtpVerify", VerifyOtp);

            userRoute.MapPost("/AddDriver", AddDriver);
            userRoute.MapGet("/GetDriverDetailsById", GetDriverDetailsByUserId);
            userRoute.MapPut("/UpdateDriver", UpdateDriver);
            userRoute.MapDelete("/DeleteDriver", DeleteDriver);
            userRoute.MapGet("/GetAllDrivers", GetAllDrivers);
            userRoute.MapGet("/GetAllCountry", GetCountryAsync);

            userRoute.MapGet("/GetAllSMSProvider", GetAllSMSProviders);
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

        internal async Task<IResult> GetUserById(Guid id, IUserService userService)
        {
            var result = await userService.GetUserByIdAsync(id);
            if(result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to Get user/User not available"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }

        internal async Task<IResult> UpdateUser(Guid id, UserDTO userDTO, IUserService userService)
        {
            var result = await userService.UpdateUserAsync(id, userDTO);
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
        private async Task<IResult> ForgotMpin(ForgotMpinDTO dto, IAuthService authService)
        {
            var (result,otp) = await authService.SendForgotMpinAsync(dto.MobileNumber);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result,$"OTP sent successfully to reset mPIN : {otp}"));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to send OTP. Please try again."));
        }

        private async Task<IResult> ResetMpin(ResetMpinDTO dto, IAuthService authService)
        {
            var result = await authService.ResetMpinAsync(dto);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result,"mPIN reset successfully."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to reset mPIN. Please try again."));
        }
        internal async Task<IResult> VerifyOtp(IAuthService authService, Guid userId, string otpCode)
        {
            var result = await authService.VerifyOtpAsync(userId, otpCode);
            if (result)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Otp Verify successful."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to verify otp"));
        }
        private async Task<IResult> GetExpenseIncomeCategoriesAsync(IMasterDataService masterDataService, int userTypeId, bool active = true)
        {
            var incomeCategories = await masterDataService.GetIncomeCategoriesAsync(userTypeId);
            var expenseCategories = await masterDataService.GetExpenseCategoriesAsync(userTypeId);

            var response = new
            {
                IncomeCategory = incomeCategories,
                ExpenseCategory = expenseCategories
            };
            //var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
            return Results.Ok(ApiResponse<object>.SuccessResponse(response));
        }
        internal async Task<IResult> AddDriver(UserDTO userDTO, IUserService userService)
        {
            if (userDTO == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("Driver details are invalid"));

            var result = await userService.AddDriverAsync(userDTO);
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to register new driver"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result,"New driver added successful."));
        }

        internal async Task<IResult> GetDriverDetailsByUserId(Guid id, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Id."));
            }

            var driver = await userService.GetDriverByIdAsync(id);
            if (driver == null)
                Results.Ok(ApiResponse<object>.FailureResponse("Driver not found"));

            return Results.Ok(ApiResponse<object>.SuccessResponse(driver, "Driver details found"));
        }

        internal async Task<IResult> UpdateDriver(Guid id, UserDTO userDTO, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Id."));
            }
            if (userDTO == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid request body"));
            }

            var updateDriver = await userService.UpdateDriverAsync(id, userDTO);
            if (updateDriver == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to update"));

            return Results.Ok(ApiResponse<object>.SuccessResponse(updateDriver, "driver update successful."));
        }

        internal async Task<IResult> DeleteDriver(Guid id, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Id."));
            }

            var result = await userService.DeleteDriverAsync(id);
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
    }
}
