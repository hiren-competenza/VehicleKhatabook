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
    public class IncomeEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            //var incomeRoute = app.MapGroup("/api/income").WithTags("Income Management");
            //incomeRoute.MapPost("/", AddIncome);
            //incomeRoute.MapGet("/{id}", GetIncomeDetails);
            //incomeRoute.MapPut("/{id}", UpdateIncome);
            //incomeRoute.MapDelete("/{id}", DeleteIncome);
            //incomeRoute.MapGet("/all", GetAllIncomes);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
        }

        //internal async Task<IResult> AddIncome(IncomeDTO incomeDTO, IIncomeService incomeService)
        //{
        //    ApiResponse<Income> result = await incomeService.AddIncomeAsync(incomeDTO);
        //    if (result.status != 200)
        //    {
        //        return Results.BadRequest(result);
        //    }
        //    return Results.Ok(result);
        //}

        //internal async Task<IResult> GetIncomeDetails(int id, IIncomeService incomeService)
        //{
        //    var result = await incomeService.GetIncomeDetailsAsync(id);
        //    return result.Success ? Results.Ok(result.Data) : Results.NotFound(result.Message);
        //}

        //internal async Task<IResult> UpdateIncome(int id, IncomeDTO incomeDTO, IIncomeService incomeService)
        //{
        //    var result = await incomeService.UpdateIncomeAsync(id, incomeDTO);
        //    return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        //}

        //internal async Task<IResult> DeleteIncome(int id, IIncomeService incomeService)
        //{
        //    var result = await incomeService.DeleteIncomeAsync(id);
        //    return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        //}

        //internal async Task<IResult> GetAllIncomes(IIncomeService incomeService)
        //{
        //    var result = await incomeService.GetAllIncomesAsync();
        //    return result.Success ? Results.Ok(result.Data) : Results.NoContent();
        //}
    }

}
