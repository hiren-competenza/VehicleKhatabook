using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class ExpenseCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Expense Category Management");//.RequireAuthorization("AdminPolicy");
            staticRoute.MapPost("/addExpenseCategory", AddExpenseCategory);
            staticRoute.MapPut("/updateExpenseCategory/{id}", UpdateExpenseCategory);
            staticRoute.MapDelete("/deleteExpenseCategory/{id}", DeleteExpenseCategory);
            staticRoute.MapGet("/GetExpenseCategories", GetExpenseCategoriesAsync);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddExpenseCategory(ExpenseCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddExpenseCategoryAsync(categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> UpdateExpenseCategory(int id, ExpenseCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateExpenseCategoryAsync(id, categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> DeleteExpenseCategory(int id, IMasterDataService masterDataService)
        {
            var result = await masterDataService.DeleteExpenseCategoryAsync(id);
            return Results.Ok(result);
        }
        internal async Task<IResult> GetExpenseCategoriesAsync(IMasterDataService masterDataService)
        {

            var response = await masterDataService.GetExpenseCategory();

            return Results.Ok(ApiResponse<object>.SuccessResponse(response));

        }
    }
}
