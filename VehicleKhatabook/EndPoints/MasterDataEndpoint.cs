using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class MasterDataEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/static").WithTags("Master Data Management");

            // Income Category Endpoints
            staticRoute.MapPost("/income-category", AddIncomeCategory);
            staticRoute.MapPut("/income-category/{id}", UpdateIncomeCategory);
            staticRoute.MapDelete("/income-category/{id}", DeleteIncomeCategory);

            // Expense Category Endpoints
            staticRoute.MapPost("/expense-category", AddExpenseCategory);
            staticRoute.MapPut("/expense-category/{id}", UpdateExpenseCategory);
            staticRoute.MapDelete("/expense-category/{id}", DeleteExpenseCategory);

            staticRoute.MapGet("/api/vehicletypes", GetVehicleTypesAsync);
            
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddIncomeCategory(IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddIncomeCategoryAsync(categoryDTO);
            return result.Success ? Results.Created($"/api/static/income-category/{result.Data.IncomeCategoryID}", result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> UpdateIncomeCategory(int id, IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateIncomeCategoryAsync(id, categoryDTO);
            return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> DeleteIncomeCategory(int id, IMasterDataService masterDataService)
        {
            var result = await masterDataService.DeleteIncomeCategoryAsync(id);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        }

        internal async Task<IResult> AddExpenseCategory(ExpenseCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddExpenseCategoryAsync(categoryDTO);
            return result.Success ? Results.Created($"/api/static/expense-category/{result.Data.ExpenseCategoryID}", result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> UpdateExpenseCategory(int id, ExpenseCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateExpenseCategoryAsync(id, categoryDTO);
            return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> DeleteExpenseCategory(int id, IMasterDataService masterDataService)
        {
            var result = await masterDataService.DeleteExpenseCategoryAsync(id);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        }
        internal async Task<IResult> GetVehicleTypesAsync(IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.GetAllVehicleTypesAsync();
            return Results.Ok(vehicleTypes);
        }
    }
}
