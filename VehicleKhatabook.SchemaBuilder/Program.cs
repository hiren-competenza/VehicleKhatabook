using VehicleKhatabook.Entities;
using VehicleKhatabook.SchemaBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

try
{
    var hostConfiguration = HostConfiguration.Build(args);
    ILogger<Program> logger = hostConfiguration.LoggerFactory.CreateLogger<Program>();
    string? connectionString = hostConfiguration.Configuration!.GetConnectionString("BonoboDb");

    VehicleKhatabookDbContextFactory factory = new();
    VehicleKhatabookDbContext dbContext = factory.CreateDbContext(args);

    var availableMigrations = dbContext.Database.GetMigrations();
    foreach (var migration in availableMigrations)
    {
        logger.LogInformation($"Available migration: {migration}");
    }

    var appliedMigrations = dbContext.Database.GetAppliedMigrations();
    foreach (var migration in appliedMigrations)
    {
        logger.LogInformation($"Previously applied migration: {migration}");
    }

    dbContext.Database.Migrate();

    await ApplyScriptsAsync(logger, connectionString!);

    Thread.Sleep(500); //sleep to let logger flush
}
catch (Exception ex)
{
    Console.WriteLine($"Error during migration: {ex.Message}");
    Console.WriteLine(ex.ToString());
}

async Task ApplyScriptsAsync(ILogger logger, string connectionString)
{
    logger.LogInformation($"Applying SQL scripts");

    var folders = new List<string>
{
    "SQL/SeedData",
    "SQL/Procedures"
};

    SqlScriptRunner scriptRunner = new SqlScriptRunner(logger, connectionString);
    await scriptRunner.ApplyScriptsAsync(folders);
}
