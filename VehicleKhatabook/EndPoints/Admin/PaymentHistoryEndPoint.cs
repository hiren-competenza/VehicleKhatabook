using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class PaymentHistoryEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Payment History");
            staticRoute.MapPost("/AddPaymentRecord", AddPaymentRecord);
            //staticRoute.MapPatch("/updateApplicationConfiguration/{id}", UpdateApplicationConfiguration);
            staticRoute.MapGet("/GetAllPaymentRecord", GetAllPaymentRecord);
            staticRoute.MapGet("/GetAllPaymentRecordByUserId", GetAllPaymentRecordByUserId);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }


        public async Task<IResult> AddPaymentRecord(PaymentHistory paymentHistory, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddPaymentRecord(paymentHistory);
            if (result.status == 200)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Payment not successfull. Contact to Helpline."));
        }

        //public async Task<IResult> UpdateLanguageType(int id, LanguageTypeDTO languageTypeDTO, ILanguageTypeService languageTypeService)
        //{
        //    var result = await languageTypeService.UpdateLanguageTypeAsync(id, languageTypeDTO);
        //    if (result.status == 200)
        //    {
        //        return Results.Ok(result);
        //    }
        //    return Results.BadRequest(result.Message);
        //}
        public async Task<IResult> GetAllPaymentRecord(IMasterDataService masterDataService)
        {
            var result = await masterDataService.GetAllPaymentRecord();
            if (!result.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Payement Record"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
        public async Task<IResult> GetAllPaymentRecordByUserId(string UserId,IMasterDataService masterDataService)
        {
            var result = await masterDataService.GetAllPaymentRecordByUserId(UserId);
            if (!result.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Payement Record"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
        //public async Task<IResult> DeletePaymentRecordById(string PayementId, IMasterDataService masterDataService)
        //{
        //    var result = await masterDataService.DeletePaymentRecordById(PayementId);
        //    if (result == null)
        //    {
        //        return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Payement Record"));
        //    }
        //    return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Payement Record Deleted Succesfully"));
        //}
    }
}
