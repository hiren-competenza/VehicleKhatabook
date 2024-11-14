using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class IncomeCategoryEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Income Category Management");//.RequireAuthorization("AdminPolicy");
            staticRoute.MapPost("/addIncomeCategory", AddIncomeCategory);
            staticRoute.MapPut("/updateIncomeCategory/{id}", UpdateIncomeCategory);
            staticRoute.MapDelete("/deleteIncomeCategory/{id}", DeleteIncomeCategory);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddIncomeCategory(IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddIncomeCategoryAsync(categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> UpdateIncomeCategory(int id, IncomeCategoryDTO categoryDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateIncomeCategoryAsync(id, categoryDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> DeleteIncomeCategory(int id, IMasterDataService masterDataService)
        {
            var result = await masterDataService.DeleteIncomeCategoryAsync(id);
            return Results.Ok(result);
        }
    }
}
