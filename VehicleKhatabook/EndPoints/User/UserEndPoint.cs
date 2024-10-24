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
            var userRoute = app.MapGroup("api/user").WithTags("User Registration and Authentication");
            userRoute.MapPost("/v1/register", UserSignup).AddEndpointFilter<ValidationFilter<UserDTO>>();
            //userRoute.MapGet("/{id:guid}", GetUserById);
            userRoute.MapPut("/UpdateUser", UpdateUser);
            //userRoute.MapDelete("/{id:guid}", DeleteUser);
            //userRoute.MapGet("/", GetAllUsers);
            userRoute.MapPost("/Login", Login);
            userRoute.MapPost("/api/auth/forgot-password", ForgotMpin);
            userRoute.MapPost("/api/auth/reset-mpin", ResetMpin);
            userRoute.MapGet("/api/GetExpenseIncomeCategoriesById", GetExpenseIncomeCategoriesAsync);

            userRoute.MapPost("/AddDriver", AddDriver);
            userRoute.MapGet("/GetDriverDetails", GetDriverDetailsByUserId);
            userRoute.MapPut("/UpdateDriver", UpdateDriver);
            userRoute.MapDelete("/DeleteDriver", DeleteDriver);
            userRoute.MapGet("/GetAllDrivers", GetAllDrivers);
            userRoute.MapGet("/api/GetAllCountry", GetCountryAsync);

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

        internal async Task<IResult> UserSignup(UserDTO userDTO, IUserService userService)
        {
            var result = await userService.CreateUserAsync(userDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> GetUserById(Guid id, IUserService userService)
        {
            var result = await userService.GetUserByIdAsync(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        internal async Task<IResult> UpdateUser(Guid id, UserDTO userDTO, IUserService userService)
        {
            var result = await userService.UpdateUserAsync(id, userDTO);
            return Results.Ok(result);
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
        internal async Task<IResult> Login(UserLoginDTO userLoginDTO, IAuthService authService)
        {
            ApiResponse<UserDetailsDTO> result = await authService.AuthenticateUser(userLoginDTO);
            if (result.Data != null && result.status == 200)
            {
                string token = authService.GenerateToken(result.Data);

                return Results.Ok(new
                {
                    Token = token,
                    UserDetails = result
                });
            }
            return Results.BadRequest(ApiResponse<UserDetailsDTO>.FailureResponse("Invalid mobile number or mPIN."));
        }
        private async Task<IResult> ForgotMpin(ForgotMpinDTO dto, IAuthService authService)
        {
            var result = await authService.SendForgotMpinAsync(dto.MobileNumber);
            if (result)
            {
                return Results.Ok("OTP sent successfully to reset mPIN.");
            }
            return Results.BadRequest("Failed to send OTP. Please try again.");
        }

        private async Task<IResult> ResetMpin(ResetMpinDTO dto, IAuthService authService)
        {
            var result = await authService.ResetMpinAsync(dto);
            if (result)
            {
                return Results.Ok("mPIN reset successfully.");
            }
            return Results.BadRequest("Failed to reset mPIN. Please try again.");
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

            return Results.Ok(response);
        }
        internal async Task<IResult> AddDriver(UserDTO userDTO, IUserService userService)
        {
            if (userDTO == null)
                return Results.BadRequest("Driver details are invalid");

            var result = await userService.AddDriverAsync(userDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> GetDriverDetailsByUserId(Guid id, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }

            var driver = await userService.GetDriverByIdAsync(id);
            return Results.Ok(driver);
        }

        internal async Task<IResult> UpdateDriver(Guid id, UserDTO userDTO, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }
            if (userDTO == null)
            {
                return Results.BadRequest("Invalid request body");
            }

            var updateDriver = await userService.UpdateDriverAsync(id, userDTO);
            return Results.Ok(updateDriver);
        }

        internal async Task<IResult> DeleteDriver(Guid id, IUserService userService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }

            var result = await userService.DeleteDriverAsync(id);
            return Results.Ok(result);
        }

        internal async Task<IResult> GetAllDrivers(IUserService userService)
        {
            var drivers = await userService.GetAllDriversAsync();
            return Results.Ok(drivers);
        }
        internal async Task<IResult> GetCountryAsync(IMasterDataService masterDataService)
        {
            var countries = await masterDataService.GetCountryAsync();
            return Results.Ok(countries);
        }
        internal async Task<IResult> GetAllSMSProviders(ISMSProviderService sMSProviderService)
        {
            var allProviders = await sMSProviderService.GetAllSMSProvidersAsync();

            var activeProvider = allProviders.Data?.FirstOrDefault(provider => provider.IsActive);

            var result = ApiResponse<List<SMSProviderDTO>>.SuccessResponse(activeProvider != null
                ? new List<SMSProviderDTO> { activeProvider }
                : new List<SMSProviderDTO>());

            return Results.Ok(result);
        }
    }
}
