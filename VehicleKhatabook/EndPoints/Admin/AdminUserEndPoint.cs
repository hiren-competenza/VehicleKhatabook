using Microsoft.AspNetCore.Builder;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class AdminUserEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/admin").WithTags("Admin Management")/*.RequireAuthorization("AdminPolicy")*/;
            staticRoute.MapPost("/Login", AdminLogin);
            //staticRoute.MapPost("/RegisterAdmin", register);
            //staticRoute.MapPost("/UpdateAdmin", UpdateAdmin);
            //staticRoute.MapGet("/GetAdminById", GetAdminById);
            //staticRoute.MapGet("/GetAllAdmins", GetAllAdmins);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        }
        internal async Task<IResult> AdminLogin(AdminLoginDTO adminLoginDTO, IAdminUserService adminService)
        {
            var result = await adminService.AuthenticateAdminAsync(adminLoginDTO);

            if (result.Success)
            {
                var responseData = new
                {
                    AdminDetails = result.AdminDetails
                };

                return Results.Ok(ApiResponse<object>.SuccessResponse(responseData, "Login successful."));
            }

            return Results.BadRequest(ApiResponse<object>.FailureResponse("Invalid mobile number or password."));
        }
        internal async Task<IResult> register(IAdminUserService adminUserService, AdminUserDTO adminDTO)
        {
            var result = await adminUserService.RegisterAdminAsync(adminDTO);
            return result != null ? Results.Ok(ApiResponse<object>.SuccessResponse(result, "Register successful.")) : 
                Results.BadRequest(ApiResponse<object>.FailureResponse("failed to register"));
        }
        internal async Task<IResult> UpdateAdmin(IAdminUserService adminUserService, AdminUserDTO adminDTO)
        {
            var result = await adminUserService.UpdateAdminAsync(adminDTO);
            if (result == null)
            {
                return Results.BadRequest(ApiResponse<object>.FailureResponse("Failed to update admin"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "update successful."));
        }
        internal async Task<IResult> GetAdminById(IAdminUserService adminUserService, int adminId)
        {
            var result = await adminUserService.GetAdminByIdAsync(adminId);
            return result != null ? Results.Ok(ApiResponse<object>.SuccessResponse(result)) :
                Results.NotFound(ApiResponse<object>.FailureResponse("Failed to get admin"));
        }
        internal async Task<IResult> GetAllAdmins(IAdminUserService adminUserService)
        {
            var result = await adminUserService.GetAllAdminsAsync();
            if (result == null)
            {
                return Results.BadRequest(ApiResponse<object>.FailureResponse("failed to load admin"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}
