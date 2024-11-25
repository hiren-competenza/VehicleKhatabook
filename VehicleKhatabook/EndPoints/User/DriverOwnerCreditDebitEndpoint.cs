using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
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
            var expenseRoute = app.MapGroup("/api/driverOwnerIncomeExpense").WithTags("Owner IncomeExpense Management").RequireAuthorization("OwnerOrDriverPolicy");
            expenseRoute.MapPost("/add", AddIncomeExpenseAsync);
            expenseRoute.MapGet("/get", GetIncomeExpenseAsyncByUserId);
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

                    var combinedResult = new
                    {
                        Income = incomeResult,
                        Expense = expenseResult
                    };

                    if (incomeResult == null && expenseResult == null)
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch income records for the user
                    var incomeResult = await ownerIncomeService.GetOwnerIncomebyUserAsync(Guid.Parse(userId));
                    if (incomeResult == null)
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(incomeResult));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch expense records for the user
                    var expenseResult = await ownerExpenseService.GetOwnerExpensebyUserAsync(Guid.Parse(userId));
                    if (expenseResult == null)
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(expenseResult));
                }
                else
                {
                    return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
                }
            }

            // Logic when DriverOwnerId is provided
            if (string.IsNullOrEmpty(transactionType))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId), start, end);
                    var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId), start, end);

                    var combinedResult = new
                    {
                        Income = incomeResult,
                        Expense = expenseResult
                    };

                    if (incomeResult == null && expenseResult == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
                }
                else
                {
                    var incomeResult = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
                    var expenseResult = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));

                    var combinedResult = new
                    {
                        Income = incomeResult,
                        Expense = expenseResult
                    };

                    if (incomeResult == null && expenseResult == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income or expense records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
                }
            }

            if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId), start, end);
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
                else
                {
                    var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(driverOwnerUserId));
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
            }
            else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId), start, end);
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
                else
                {
                    var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(driverOwnerUserId));
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse("No expense records found."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
            }
            else
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid transaction type."));
            }
        }
    }
}
