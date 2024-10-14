using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class ExpenseEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var expenseRoute = app.MapGroup("/api/expense").WithTags("Expense Management");
            expenseRoute.MapPost("/", AddExpense);
            expenseRoute.MapGet("/GetExpenseDetailsById", GetExpenseDetails);
            expenseRoute.MapPut("/UpdateExpense", UpdateExpense);
            expenseRoute.MapDelete("/DeleteExpense", DeleteExpense);
            expenseRoute.MapGet("/all", GetAllExpenses);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
        }

        internal async Task<IResult> AddExpense(ExpenseDTO expenseDTO, IExpenseService expenseService)
        {
            var result = await expenseService.AddExpenseAsync(expenseDTO);
            return result.Success ? Results.Created($"/api/expense/{result.Data.ExpenseID}", result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> GetExpenseDetails(int id, IExpenseService expenseService)
        {
            var result = await expenseService.GetExpenseDetailsAsync(id);
            return result.Success ? Results.Ok(result.Data) : Results.NotFound(result.Message);
        }

        internal async Task<IResult> UpdateExpense(int id, ExpenseDTO expenseDTO, IExpenseService expenseService)
        {
            var result = await expenseService.UpdateExpenseAsync(id, expenseDTO);
            return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> DeleteExpense(int id, IExpenseService expenseService)
        {
            var result = await expenseService.DeleteExpenseAsync(id);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        }

        internal async Task<IResult> GetAllExpenses(IExpenseService expenseService)
        {
            var result = await expenseService.GetAllExpensesAsync();
            return result.Success ? Results.Ok(result.Data) : Results.NoContent();
        }
    }
}
