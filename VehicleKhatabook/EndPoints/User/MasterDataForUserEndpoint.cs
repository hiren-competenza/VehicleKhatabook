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
    public class MasterDataForUserEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/user/master").WithTags("Master Data for Users");//.RequireAuthorization("OwnerOrDriverPolicy");
            userRoute.MapGet("/getExpenseIncomeCategoriesById", GetExpenseIncomeCategoriesAsync).RequireAuthorization("OwnerOrDriverPolicy");
            userRoute.MapGet("/getAllCountry", GetCountryAsync);
            userRoute.MapGet("/GetAllLanguageTypes", GetAllLanguageTypes);
            userRoute.MapGet("/vehicletypes", GetVehicleTypesAsync).RequireAuthorization("OwnerOrDriverPolicy");
            userRoute.MapGet("/allVehicletypes", GetVehicleAllTypesAsync); //Additional
            userRoute.MapGet("/getAllExpenseIncomeCategoriesById", GetAllExpenseIncomeCategoriesAsync).RequireAuthorization("OwnerOrDriverPolicy");//additional

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
            services.AddScoped<ILanguageTypeService, LanguageTypeService>();
            services.AddScoped<ILanguageTypeRepository, LanguageTypeRepository>();
        }
        private async Task<IResult> GetAllExpenseIncomeCategoriesAsync(HttpContext httpContext, IUserService userService, IMasterDataService masterDataService, bool active = true)
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
        internal async Task<IResult> GetCountryAsync(IMasterDataService masterDataService)
        {
            var countries = await masterDataService.GetCountryAsync();
            if (countries.Count == 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("not found country list"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(countries));
        }
        public async Task<IResult> GetAllLanguageTypes(ILanguageTypeService languageTypeService)
        {
            var result = await languageTypeService.GetAllLanguageTypesAsync();
            if (!result.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Language List"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
        internal async Task<IResult> GetVehicleAllTypesAsync(IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.GetAllVehicleTypesAsync();
            if (!vehicleTypes.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Vehicle List"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(vehicleTypes));
        }

        internal async Task<IResult> GetVehicleTypesAsync(HttpContext httpContext, IUserService userService, IMasterDataService masterDataService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var user = await userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user != null)
            {
                var vehicleTypes = await masterDataService.GetVehicleTypeForuserlanguageAsync(user.LanguageTypeId ?? 0);
                if (!vehicleTypes.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Vehicle List"));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(vehicleTypes));
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User Not Found"));
            }
        }
        private async Task<IResult> GetExpenseIncomeCategoriesAsync(HttpContext httpContext, IUserService userService, IMasterDataService masterDataService, bool active = true)
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
                var incomeCategories = await masterDataService.GetIncomeCategoriesForuserlanguageAsync(user.UserTypeId, user.LanguageTypeId ?? 0);
                var expenseCategories = await masterDataService.GetExpenseCategoriesForuserlanguageAsync(user.UserTypeId, user.LanguageTypeId ?? 0);

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
    }
}
