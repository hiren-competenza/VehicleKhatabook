using FluentValidation;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Models.Filters;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class UserEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/user").WithTags("User Registration and Authentication");
            userRoute.MapPost("/v1/register", UserSignup).AddEndpointFilter<ValidationFilter<UserDTO>>();
            //userRoute.MapGet("/{id:guid}", GetUserById);
            //userRoute.MapPut("/{id:guid}", UpdateUser);
            //userRoute.MapDelete("/{id:guid}", DeleteUser);
            //userRoute.MapGet("/", GetAllUsers);
            userRoute.MapPost("/Login",Login);
            userRoute.MapPost("/api/auth/forgot-password", ForgotMpin);
            userRoute.MapPost("/api/auth/reset-mpin", ResetMpin);
            userRoute.MapGet("/api/GetExpenseIncomeCategoriesById", GetExpenseIncomeCategoriesAsync);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddValidatorsFromAssemblyContaining<AddUserValidator>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOTPService, OTPService>();
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
            return result != null ? Results.Ok(result) : Results.NotFound();
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
            try
            {
                var user = await authService.AuthenticateUser(userLoginDTO);
                return Results.Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                return Results.Problem("An error occurred while authenticating.");
            }
        }
        private async Task<IResult> ForgotMpin(ForgotMpinDTO dto, IAuthService authService)
        {
            var result = await authService.SendForgotMpinEmailAsync(dto.Email);
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
            var incomeCategories =  await masterDataService.GetIncomeCategoriesAsync(userTypeId);
            var expenseCategories = await masterDataService.GetExpenseCategoriesAsync(userTypeId);

            var response = new
            {
                IncomeCategory = incomeCategories,
                ExpenseCategory = expenseCategories 
            };
            //var jsonResponse = JsonConvert.SerializeObject(response, Formatting.Indented);

            return Results.Ok(response);
        }
    }
}
