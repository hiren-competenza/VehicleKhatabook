using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class MasterDataEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/static").WithTags("Master Data Management").RequireAuthorization("OwnerOrDriverPolicy");

            // Income Category Endpoints
            staticRoute.MapPost("/income-category", AddIncomeCategory);
            staticRoute.MapPut("/income-category/{id}", UpdateIncomeCategory);
            staticRoute.MapDelete("/income-category/{id}", DeleteIncomeCategory);

            // Expense Category Endpoints
            staticRoute.MapPost("/expense-category", AddExpenseCategory);
            staticRoute.MapPut("/expense-category/{id}", UpdateExpenseCategory);
            staticRoute.MapDelete("/expense-category/{id}", DeleteExpenseCategory);

            staticRoute.MapPost("/api/AddVehicleTypes", AddVehicleTypesAsync);
            staticRoute.MapPost("/api/UpdateVehicleType", UpdateVehicleTypeAsync);
            staticRoute.MapGet("/api/vehicletypes", GetVehicleTypesAsync);

            staticRoute.MapGet("/api/GetAllCountry", GetCountryAsync);

        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddIncomeCategory(IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddIncomeCategoryAsync(categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> UpdateIncomeCategory(int id, IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateIncomeCategoryAsync(id, categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> DeleteIncomeCategory(int id, IMasterDataService masterDataService)
        {
            var result = await masterDataService.DeleteIncomeCategoryAsync(id);
            return Results.Ok(result);
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

        internal async Task<IResult> AddVehicleTypesAsync(VechileType vechileType, IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.AddVehicleTypesAsync(vechileType);
            return Results.Ok(vehicleTypes);
        }
        internal async Task<IResult> UpdateVehicleTypeAsync(int vehicleTypeId, VechileType vehicleTypeDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateVehicleTypeAsync(vehicleTypeId, vehicleTypeDTO);
            return Results.Ok(result);
        }
        internal async Task<IResult> GetVehicleTypesAsync(IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.GetAllVehicleTypesAsync();
            return Results.Ok(vehicleTypes);
        }
        internal async Task<IResult> GetCountryAsync(IMasterDataService masterDataService)
        {
            var countries = await masterDataService.GetCountryAsync();
            return Results.Ok(countries);
        }
    }
}
