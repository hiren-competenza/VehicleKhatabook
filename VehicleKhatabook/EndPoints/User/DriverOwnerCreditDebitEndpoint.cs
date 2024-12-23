using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
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
    public class DriverOwnerCreditDebitEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var expenseRoute = app.MapGroup("/api/driverOwnerIncomeExpense").WithTags("Owner IncomeExpense Management").RequireAuthorization("OwnerOrDriverPolicy"); ;
            expenseRoute.MapPost("/add", AddIncomeExpenseAsync);
            expenseRoute.MapGet("/get", GetIncomeExpenseAsyncByUserId);
            expenseRoute.MapPut("/update", UpdateIncomeExpenseAsync);
            expenseRoute.MapDelete("/Delete", DeleteIncomeExpenseAsync);
            expenseRoute.MapPost("/settlement", AccountSettlement);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOwnerIncomeRepository, OwnerIncomeRepository>();
            services.AddScoped<IOwnerExpenseRepository, OwnerExpenseRepository>();
            services.AddScoped<IOwnerIncomeService, OwnerIncomeService>();
            services.AddScoped<IOwnerExpenseService, OwnerExpenseService>();
        }
        internal async Task<IResult> AddIncomeExpenseAsync(HttpContext httpContext, string TransactionType, OwnerIncomeExpenseDTO ownerIncomeExpenseDTO, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var ownerDTO = new OwnerIncomeExpenseDTO
            {
                //Name = ownerIncomeExpenseDTO.Name,
                //UserId = Guid.Parse(userId),
                //Mobile = ownerIncomeExpenseDTO.Mobile,
                Date = ownerIncomeExpenseDTO.Date,
                Amount = ownerIncomeExpenseDTO.Amount,
                Note = ownerIncomeExpenseDTO.Note,
                DriverOwnerUserId = ownerIncomeExpenseDTO.DriverOwnerUserId,
            };
            if (TransactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                OwnerKhataCredit result = await ownerIncomeService.AddOwnerIncomeAsync(ownerDTO);
                if (result == null)
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Failed to add income"));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Income added successfully."));
            }
            else
            {
                OwnerKhataDebit result = await ownerExpenseService.AddOwnerExpenseAsync(ownerDTO);
                if (result == null)
                    return Results.Ok(ApiResponse<object>.FailureResponse("failed to add expense."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Expense added  successful."));
            }
        }
        internal async Task<IResult> UpdateIncomeExpenseAsync(Guid userId, string TransactionType, OwnerIncomeExpenseDTO ownerIncomeExpenseDTO, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService)
        {
            if (userId == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User ID is required."));
            }

            var ownerDTO = new OwnerIncomeExpenseDTO
            {
                Date = ownerIncomeExpenseDTO.Date,
                Amount = ownerIncomeExpenseDTO.Amount,
                Note = ownerIncomeExpenseDTO.Note,
                DriverOwnerUserId = ownerIncomeExpenseDTO.DriverOwnerUserId,
            };

            if (TransactionType.ToLower() == TransactionTypeEnum.Credit.ToLower())
            {
                OwnerKhataCredit result = await ownerIncomeService.UpdateOwnerIncomeAsync(userId, ownerDTO);
                if (result == null)
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Failed to update income."));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Income updated successfully."));
            }
            else
            {
                OwnerKhataDebit result = await ownerExpenseService.UpdateOwnerExpenseAsync(userId, ownerDTO);
                if (result == null)
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Failed to update expense."));
                }
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Expense updated successfully."));
            }
        }


        internal async Task<IResult> DeleteIncomeExpenseAsync(
     [FromQuery] Guid userId,
     [FromQuery] string TransactionType,
     [FromServices] IOwnerIncomeService ownerIncomeService,
     [FromServices] IOwnerExpenseService ownerExpenseService)
        {
            if (userId == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User ID is required."));
            }

            if (string.IsNullOrEmpty(TransactionType))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Transaction type is required."));
            }

            if (TransactionType.Equals(TransactionTypeEnum.Credit, StringComparison.OrdinalIgnoreCase))
            {
                var result = await ownerIncomeService.DeleteOwnerIncomeAsync(userId);
                return result
                    ? Results.Ok(ApiResponse<object>.SuccessResponse(null, "Income deleted successfully."))
                    : Results.Ok(ApiResponse<object>.FailureResponse("Failed to delete income."));
            }
            else if (TransactionType.Equals(TransactionTypeEnum.Debit, StringComparison.OrdinalIgnoreCase))
            {
                var result = await ownerExpenseService.DeleteOwnerExpenseAsync(userId);
                return result
                    ? Results.Ok(ApiResponse<object>.SuccessResponse(null, "Expense deleted successfully."))
                    : Results.Ok(ApiResponse<object>.FailureResponse("Failed to delete expense."));
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
            }
        }

        #region Working code with all parameter compulsory
        //internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string transactionType, string driverOwnerUserId, HttpContext httpContext, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService, DateTime? fromDate, DateTime? toDate)
        //{
        //    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
        //    }

        //    // Set the date range to the current day if fromDate and toDate are not provided
        //    var start = fromDate ?? DateTime.UtcNow.Date;
        //    var end = toDate ?? DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        //    // Fetch data based on transaction type and provided date range
        //    if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
        //    {
        //        var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId), start, end);
        //        if (result == null)
        //            return Results.Ok(ApiResponse<object>.FailureResponse($"No income records found for user ID {userId}."));

        //        return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //    }
        //    else
        //    {
        //        var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId), start, end);
        //        if (result == null)
        //            return Results.Ok(ApiResponse<object>.FailureResponse($"No expense records found for user ID {userId}."));

        //        return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //    }
        //} 
        #endregion
        #region Working without date filter.
        //internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string? transactionType, string? driverOwnerUserId, HttpContext httpContext, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService, DateTime? fromDate, DateTime? toDate)
        //{
        //    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
        //    }

        //    if (string.IsNullOrEmpty(driverOwnerUserId))
        //    {
        //        // Handle scenario when DriverOwnerId is not provided
        //        if (string.IsNullOrEmpty(transactionType))
        //        {
        //            // Fetch both income and expense records for the user
        //            var incomeResult = await ownerIncomeService.GetOwnerIncomebyUserAsync(Guid.Parse(userId));
        //            var expenseResult = await ownerExpenseService.GetOwnerExpensebyUserAsync(Guid.Parse(userId));

        //            var combinedResult = new
        //            {
        //                Income = incomeResult,
        //                Expense = expenseResult
        //            };

        //            if (incomeResult == null && expenseResult == null)
        //            {
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));
        //            }

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
        //        }
        //        else if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Fetch income records for the user
        //            var incomeResult = await ownerIncomeService.GetOwnerIncomebyUserAsync(Guid.Parse(userId));
        //            if (incomeResult == null)
        //            {
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
        //            }

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(incomeResult));
        //        }
        //        else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Fetch expense records for the user
        //            var expenseResult = await ownerExpenseService.GetOwnerExpensebyUserAsync(Guid.Parse(userId));
        //            if (expenseResult == null)
        //            {
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));
        //            }

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(expenseResult));
        //        }
        //        else
        //        {
        //            return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
        //        }
        //    }

        //    // Logic when DriverOwnerId is provided
        //    if (string.IsNullOrEmpty(transactionType))
        //    {
        //        if (fromDate.HasValue && toDate.HasValue)
        //        {
        //            var start = fromDate.Value.Date;
        //            var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

        //            var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId), start, end);
        //            var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId), start, end);

        //            var combinedResult = new
        //            {
        //                Income = incomeResult,
        //                Expense = expenseResult
        //            };

        //            if (incomeResult == null && expenseResult == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
        //        }
        //        else
        //        {
        //            var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
        //            var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));

        //            var combinedResult = new
        //            {
        //                Income = incomeResult,
        //                Expense = expenseResult
        //            };

        //            if (incomeResult == null && expenseResult == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
        //        }
        //    }

        //    if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
        //    {
        //        if (fromDate.HasValue && toDate.HasValue)
        //        {
        //            var start = fromDate.Value.Date;
        //            var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

        //            var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId), start, end);
        //            if (result == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //        }
        //        else
        //        {
        //            var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
        //            if (result == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //        }
        //    }
        //    else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
        //    {
        //        if (fromDate.HasValue && toDate.HasValue)
        //        {
        //            var start = fromDate.Value.Date;
        //            var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

        //            var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId), start, end);
        //            if (result == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //        }
        //        else
        //        {
        //            var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));
        //            if (result == null)
        //                return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));

        //            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        //        }
        //    }
        //    else
        //    {
        //        return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
        //    }
        //}

        #endregion
        internal async Task<IResult> AccountSettlement(string? driverOwnerUserId, HttpContext httpContext, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            if (string.IsNullOrEmpty(driverOwnerUserId) || !Guid.TryParse(driverOwnerUserId, out var driverOwnerUserGuid))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Driver/Owner User ID."));
            }
            // Call the services to delete both debit and credit account data
            var incomeResult = await ownerIncomeService.AccountSettlementIncomeAsync(driverOwnerUserGuid, Guid.Parse(userId));
            var expenseResult = await ownerExpenseService.AccountSettlementExpenseAsync(driverOwnerUserGuid, Guid.Parse(userId));

            // If both operations succeed
            if (incomeResult && expenseResult)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(true, "Account settlement completed successfully."));
            }
            // If either operation fails
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to settle account.  Please try again later."));
        }

        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string? transactionType, string? driverOwnerUserId, HttpContext httpContext, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService, DateTime? fromDate, DateTime? toDate)
        {

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            if (string.IsNullOrEmpty(driverOwnerUserId))
            {
                // Handle scenario when DriverOwnerId is not provided
                if (string.IsNullOrEmpty(transactionType))
                {
                    // Fetch both income and expense records for the user
                    var incomeResult = await ownerIncomeService.GetOwnerIncomebyUserAsync(Guid.Parse(userId));
                    var expenseResult = await ownerExpenseService.GetOwnerExpensebyUserAsync(Guid.Parse(userId));

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
                    var incomeResult = await ownerIncomeService.GetOwnerIncomebyUserAsync(Guid.Parse(userId));
                    var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);

                    if (!filteredIncome.Any())
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(filteredIncome));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var expenseResult = await ownerExpenseService.GetOwnerExpensebyUserAsync(Guid.Parse(userId));
                    var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                    if (!filteredExpense.Any())
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(filteredExpense));
                }
                else
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
                }
            }

            // Logic when DriverOwnerId is provided
            if (string.IsNullOrEmpty(transactionType))
            {
                var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
                var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));

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

            if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
                var filteredIncome = FilterByDate(incomeResult, fromDate, toDate);

                if (!filteredIncome.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
                }

                return Results.Ok(ApiResponse<object>.SuccessResponse(filteredIncome));
            }
            else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));
                var filteredExpense = FilterByDate(expenseResult, fromDate, toDate);

                if (!filteredExpense.Any())
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));
                }

                return Results.Ok(ApiResponse<object>.SuccessResponse(filteredExpense));
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
            }
        }

        // Updated FilterByDate Method
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
