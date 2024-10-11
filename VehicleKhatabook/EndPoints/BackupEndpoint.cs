using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class BackupEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var backups = app.MapGroup("/api/backup").WithTags("Backup & Restore");

            backups.MapPost("/", BackupData);
            backups.MapPost("/restore", RestoreData);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBackupRepository, BackupRepository>();
            services.AddScoped<IBackupService, BackupService>();
        }

        private async Task<IResult> BackupData(Guid userId, HttpRequest request, IBackupService backupService)
        {
            using var memoryStream = new MemoryStream();
            await request.Body.CopyToAsync(memoryStream);
            var data = memoryStream.ToArray();

            var backup = await backupService.BackupDataAsync(userId, data);
            return Results.Ok(backup);
        }

        private async Task<IResult> RestoreData(Guid userId, Guid backupId, IBackupService backupService)
        {
            var backup = await backupService.RestoreDataAsync(userId, backupId);
            if (backup != null)
            {
                return Results.Ok(backup);
            }

            return Results.NotFound("Backup not found.");
        }
    }
}
