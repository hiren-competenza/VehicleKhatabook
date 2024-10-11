﻿using VehicleKhatabook.Infrastructure;
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
            staticRoute.MapGet("/income-categories", GetIncomeCategories);
            staticRoute.MapPost("/income-category", AddIncomeCategory);
            staticRoute.MapPut("/income-category/{id}", UpdateIncomeCategory);
            staticRoute.MapDelete("/income-category/{id}", DeleteIncomeCategory);

            // Expense Category Endpoints
            staticRoute.MapGet("/expense-categories", GetExpenseCategories);
            staticRoute.MapPost("/expense-category", AddExpenseCategory);
            staticRoute.MapPut("/expense-category/{id}", UpdateExpenseCategory);
            staticRoute.MapDelete("/expense-category/{id}", DeleteExpenseCategory);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        // Income Category Endpoints Handlers
        internal async Task<IResult> GetIncomeCategories(IMasterDataService masterDataService)
        {
            var result = await masterDataService.GetIncomeCategoriesAsync();
            return result.Success ? Results.Ok(result.Data) : Results.NoContent();
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

        // Expense Category Endpoints Handlers
        internal async Task<IResult> GetExpenseCategories(IMasterDataService masterDataService)
        {
            var result = await masterDataService.GetExpenseCategoriesAsync();
            return result.Success ? Results.Ok(result.Data) : Results.NoContent();
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
    }
}