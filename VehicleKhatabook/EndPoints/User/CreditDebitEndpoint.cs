﻿using System.Security.Claims;
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

            // Handle scenario when vehicleId is not provided
            if (string.IsNullOrEmpty(vehicleId))
            {
                if (string.IsNullOrEmpty(transactionType))
                {
                    // Fetch both income and expense records for the user
                    var incomeResult = await incomeService.GetIncomebyUserAsync(Guid.Parse(userId));
                    var expenseResult = await expenseService.GetExpensebyUserAsync(Guid.Parse(userId));

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
                    var incomeResult = await incomeService.GetIncomebyUserAsync(Guid.Parse(userId));
                    if (incomeResult == null)
                    {
                        return Results.Ok(ApiResponse<object>.FailureResponse("No income records found."));
                    }

                    return Results.Ok(ApiResponse<object>.SuccessResponse(incomeResult));
                }
                else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    // Fetch expense records for the user
                    var expenseResult = await expenseService.GetExpensebyUserAsync(Guid.Parse(userId));
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

            // Existing logic when vehicleId is provided
            if (string.IsNullOrEmpty(transactionType))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var incomeResult = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId), start, end);
                    var expenseResult = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId), start, end);

                    var combinedResult = new
                    {
                        Income = incomeResult,
                        Expense = expenseResult
                    };

                    if (incomeResult == null && expenseResult == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No income or expense records found for user ID {userId}."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
                }
                else
                {
                    var incomeResult = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId));
                    var expenseResult = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId));

                    var combinedResult = new
                    {
                        Income = incomeResult,
                        Expense = expenseResult
                    };

                    if (incomeResult == null && expenseResult == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No income or expense records found for user ID {userId}."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(combinedResult));
                }
            }

            if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var result = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId), start, end);
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No income records found for user ID {userId}."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
                else
                {
                    var result = await incomeService.GetIncomeAsync(Guid.Parse(vehicleId));
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No income records found for user ID {userId}."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
            }
            else if (transactionType.Equals(TransactionTypeEnum.Debit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1).AddTicks(-1);

                    var result = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId), start, end);
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No expense records found for user ID {userId}."));

                    return Results.Ok(ApiResponse<object>.SuccessResponse(result));
                }
                else
                {
                    var result = await expenseService.GetExpenseAsync(Guid.Parse(vehicleId));
                    if (result == null)
                        return Results.Ok(ApiResponse<object>.FailureResponse($"No expense records found for user ID {userId}."));

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
