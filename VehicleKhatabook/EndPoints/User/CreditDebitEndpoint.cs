using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class CreditDebitEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var expenseRoute = app.MapGroup("/api/incomeExpense").WithTags("IncomeExpense Management").RequireAuthorization("OwnerOrDriverPolicy");
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

                Income result = await incomeService.AddIncomeAsync(incomeDTO);
                if(result == null)
                {
                    return Results.BadRequest(ApiResponse<object>.FailureResponse("failed to add income"));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(result,"Income added successful."));
            }
            else
            {
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

                Expense result = await expenseService.AddExpenseAsync(expenseDTO);
                if (result == null)
                    return Results.BadRequest(ApiResponse<object>.FailureResponse("failed to add expense."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Expense added  successful."));
            }
        }
        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string transactionType, Guid userId, IIncomeService incomeService, IExpenseService expenseService)
        {
            if (transactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                var result = await incomeService.GetIncomeAsync(userId);
                if (result == null)
                    return Results.BadRequest(ApiResponse<object>.FailureResponse($"No income records found for user ID {userId}."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result));
            }
            else
            {
                var result = await expenseService.GetExpenseAsync(userId);
                if (result == null)
                    return Results.BadRequest(ApiResponse<object>.FailureResponse($"No expense records found for user ID {userId}."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result));
            }
        }
    }
}
