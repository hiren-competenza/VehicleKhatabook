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
            services.AddScoped<ICreditDebitService, CreditDebitService>();
            services.AddScoped<ICreditDebitRepositories, CreditDebitRepositories>();
        }
        internal async Task<IResult> AddIncomeExpenseAsync(IncomeExpenseDTO IncomeExpenseDTO, ICreditDebitService creditDebitService)
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

                var result = await creditDebitService.AddIncomeAsync(incomeDTO);
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

                var result = await creditDebitService.AddExpenseAsync(expenseDTO);
                return result.Success ? Results.Created($"/api/expense/{result.Data.ExpenseID}", result.Data) : Results.Conflict(result.Message);
            }
        }
        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string transactionType, Guid userId, ICreditDebitService creditDebitService)
        {
            if (transactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                var result = await creditDebitService.GetIncomeAsync(userId);
                return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
            }
            else
            {
                var result = await creditDebitService.GetExpenseAsync(userId);
                return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
            }
        }
    }
}
