using System.Security.Claims;
using VehicleKhatabook.Entities;
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
        internal async Task<IResult> AddIncomeExpenseAsync(HttpContext httpContext, IncomeExpenseDTO IncomeExpenseDTO, IIncomeService incomeService, IExpenseService expenseService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            if (IncomeExpenseDTO.TransactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                var incomeDTO = new IncomeDTO
                {
                    IncomeCategoryID = IncomeExpenseDTO.CategoryID,
                    IncomeAmount = IncomeExpenseDTO.Amount,
                    IncomeDate = IncomeExpenseDTO.Date,
                    IncomeDescription = IncomeExpenseDTO.Description,
                    IncomeVehicleId = IncomeExpenseDTO.VehicleId,
                    //DriverID = IncomeExpenseDTO.DriverID,
                    //UserId = Guid.Parse(userId),
                    CreatedBy = IncomeExpenseDTO.CreatedBy,
                    ModifiedBy = IncomeExpenseDTO.ModifiedBy

                };

                UserIncome result = await incomeService.AddIncomeAsync(incomeDTO);
                if (result == null)
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("failed to add income"));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Income added successful."));
            }
            else
            {
                var expenseDTO = new ExpenseDTO
                {
                    ExpenseCategoryID = IncomeExpenseDTO.CategoryID,
                    ExpenseAmount = IncomeExpenseDTO.Amount,
                    ExpenseDate = IncomeExpenseDTO.Date,
                    ExpenseDescription = IncomeExpenseDTO.Description,
                    //DriverID = IncomeExpenseDTO.DriverID,
                    ExpenseVehicleId = IncomeExpenseDTO.VehicleId,
                    //UserId = Guid.Parse(userId),
                    CreatedBy = IncomeExpenseDTO.CreatedBy,
                    ModifiedBy = IncomeExpenseDTO.ModifiedBy
                };

                UserExpense result = await expenseService.AddExpenseAsync(expenseDTO);
                if (result == null)
                    return Results.Ok(ApiResponse<object>.FailureResponse("failed to add expense."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Expense added  successful."));
            }
        }

        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string? transactionType, string? vehicleId, HttpContext httpContext, IIncomeService incomeService, IExpenseService expenseService, DateTime? fromDate, DateTime? toDate)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            // Fetch data based on vehicleId and transactionType
            if (string.IsNullOrEmpty(vehicleId))
            {
                if (string.IsNullOrEmpty(transactionType))
                {
                    var incomeResult = await incomeService.GetIncomebyUserAsync(Guid.Parse(userId));
                    var expenseResult = await expenseService.GetExpensebyUserAsync(Guid.Parse(userId));

                    var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);
                    var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                    if (!filteredIncome.Any() && !filteredExpense.Any())
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(new
                    {
                        Income = filteredIncome,
                        Expense = filteredExpense
                    }));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var incomeResult = await incomeService.GetIncomebyUserAsync(Guid.Parse(userId));
                    var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);

                    if (!filteredIncome.Any())
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(filteredIncome));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var expenseResult = await expenseService.GetExpensebyUserAsync(Guid.Parse(userId));
                    var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                    if (!filteredExpense.Any())
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(filteredExpense));
                }

                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
            }

            if (string.IsNullOrEmpty(transactionType))
            {
                var incomeResult = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId));
                var expenseResult = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId));

                var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);
                var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                if (!filteredIncome.Any() && !filteredExpense.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse($"No income or expense records found."));
                }

                return Results.Ok(ApiResponse<object>.SuccessResponse(new
                {
                    Income = filteredIncome,
                    Expense = filteredExpense
                }));
            }
            else if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                var incomeResult = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId));
                var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);

                if (!filteredIncome.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse($"No income records found."));
                }

                return Results.Ok(ApiResponse<object>.SuccessResponse(filteredIncome));
            }
            else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                var expenseResult = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId));
                var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                if (!filteredExpense.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse($"No expense records found."));
                }

                return Results.Ok(ApiResponse<object>.SuccessResponse(filteredExpense));
            }

            return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
        }

        // Helper method to filter records by date range
        private IEnumerable<T> FilterByDate<T>(IEnumerable<T> records, DateTime? fromDate, DateTime? toDate) where T : IHasTransactionDate
        {
            if (records == null)
            {
                return Enumerable.Empty<T>();
            }

            if (fromDate.HasValue && toDate.HasValue)
            {
                return records.Where(r => r.TransactionDate >= fromDate.Value && r.TransactionDate <= toDate.Value);
            }

            return records;
        }
    }
}
