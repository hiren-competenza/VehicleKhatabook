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
    public class OwnerCreditDebitEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var expenseRoute = app.MapGroup("/api/ownerincomeExpense").WithTags("Owner IncomeExpense Management").RequireAuthorization("OwnerOrDriverPolicy");
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
                Name = ownerIncomeExpenseDTO.Name,
                UserId = Guid.Parse(userId),
                Mobile = ownerIncomeExpenseDTO.Mobile,
                Date = ownerIncomeExpenseDTO.Date,
                Amount = ownerIncomeExpenseDTO.Amount,
                Note = ownerIncomeExpenseDTO.Note,
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
        internal async Task<IResult> GetIncomeExpenseAsyncByUserId(string transactionType, HttpContext httpContext, IOwnerIncomeService ownerIncomeService, IOwnerExpenseService ownerExpenseService, DateTime? fromDate, DateTime? toDate)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            // Set the date range to the current day if fromDate and toDate are not provided
            var start = fromDate ?? DateTime.UtcNow.Date;
            var end = toDate ?? DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

            // Fetch data based on transaction type and provided date range
            if (transactionType.Equals(TransactionTypeEnum.Credit.ToLower(), StringComparison.OrdinalIgnoreCase))
            {
                var result = await ownerIncomeService.GetOwnerIncomeAsync(Guid.Parse(userId), start, end);
                if (result == null)
                    return Results.Ok(ApiResponse<object>.FailureResponse($"No income records found for user ID {userId}."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result));
            }
            else
            {
                var result = await ownerExpenseService.GetOwnerExpenseAsync(Guid.Parse(userId), start, end);
                if (result == null)
                    return Results.Ok(ApiResponse<object>.FailureResponse($"No expense records found for user ID {userId}."));

                return Results.Ok(ApiResponse<object>.SuccessResponse(result));
            }
        }

    }
}
