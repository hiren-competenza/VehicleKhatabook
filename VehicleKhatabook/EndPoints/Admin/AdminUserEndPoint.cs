using VehicleKhatabook.Infrastructure;
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
            staticRoute.MapPost("/RegisterAdmin", register);
            staticRoute.MapPost("/UpdateAdmin", UpdateAdmin);
            staticRoute.MapGet("/GetAdminById", GetAdminById);
            staticRoute.MapGet("/GetAllAdmins", GetAllAdmins);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        }
        internal async Task<IResult> register(IAdminUserService adminUserService, AdminUserDTO adminDTO)
        {
            var result = await adminUserService.RegisterAdminAsync(adminDTO);
            return result.status == 200 ? Results.Ok(result) : Results.BadRequest(result);
        }
        internal async Task<IResult> UpdateAdmin(IAdminUserService adminUserService, AdminUserDTO adminDTO)
        {
            var result = await adminUserService.UpdateAdminAsync(adminDTO);
            return Results.Ok(result);
        }
        internal async Task<IResult> GetAdminById(IAdminUserService adminUserService, int adminId)
        {
            var result = await adminUserService.GetAdminByIdAsync(adminId);
            return result.status == 200 ? Results.Ok(result) : Results.NotFound(result);
        }
        internal async Task<IResult> GetAllAdmins(IAdminUserService adminUserService)
        {
            var result = await adminUserService.GetAllAdminsAsync();
            return Results.Ok(result);
        }
    }
}
