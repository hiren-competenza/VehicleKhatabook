using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class CreditDebitEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var expenseRoute = app.MapGroup("/api/incomeExpense").WithTags("IncomeExpense Management");
            expenseRoute.MapPost("/", AddIncomeExpenseAsync);
            expenseRoute.MapGet("/GetIncomeExpenseAsyncByUserId", GetIncomeExpenseAsyncByUserId);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IExpenseService, ExpenseService>();
        }
        internal async Task<IResult> AddIncomeExpenseAsync(IncomeExpenseDTO IncomeExpenseDTO, IIncomeService incomeService, IExpenseService expenseService)
        {
            if (IncomeExpenseDTO.TransactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                //Need to verify IncomeCategoryID
                var incomeDTO = new IncomeDTO
                {
                    IncomeCategoryID = IncomeExpenseDTO.CategoryID,
                    IncomeAmount = IncomeExpenseDTO.Amount,
                    IncomeDate = IncomeExpenseDTO.Date,
                    IncomeDescription = IncomeExpenseDTO.Description,
                    DriverID = IncomeExpenseDTO.DriverID,
                    CreatedBy = IncomeExpenseDTO.CreatedBy,
                    ModifiedBy = IncomeExpenseDTO.ModifiedBy
                };

                var result = await incomeService.AddIncomeAsync(incomeDTO);
                return result.Success ? Results.Created($"/api/income/{result.Data.IncomeID}", result.Data) : Results.Conflict(result.Message);
            }
            else
            {
                //Need to verify expenseCategoryID
                var expenseDTO = new ExpenseDTO
                {
                    ExpenseCategoryID = IncomeExpenseDTO.CategoryID,
                    ExpenseAmount = IncomeExpenseDTO.Amount,
                    ExpenseDate = IncomeExpenseDTO.Date,
                    ExpenseDescription = IncomeExpenseDTO.Description,
                    DriverID = IncomeExpenseDTO.DriverID,
                    CreatedBy = IncomeExpenseDTO.CreatedBy,
                    ModifiedBy = IncomeExpenseDTO.ModifiedBy
                };

                var result = await expenseService.AddExpenseAsync(expenseDTO);
                return result.Success ? Results.Created($"/api/expense/{result.Data.ExpenseID}", result.Data) : Results.Conflict(result.Message);
            }
        }
        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string transactionType, Guid userId, IIncomeService incomeService, IExpenseService expenseService)
        {
            if (transactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                var result = await incomeService.GetIncomeAsync(userId);
                return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
            }
            else
            {
                var result = await expenseService.GetExpenseAsync(userId);
                return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
            }
        }
    }
}
