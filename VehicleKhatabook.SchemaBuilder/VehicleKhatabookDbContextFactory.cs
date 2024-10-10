using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using VehicleKhatabook.Entities;

namespace VehicleKhatabook.SchemaBuilder
{
    public class VehicleKhatabookDbContextFactory : IDesignTimeDbContextFactory<VehicleKhatabookDbContext>
    {
        public VehicleKhatabookDbContext CreateDbContext(string[] args)
        {
            var hostConfiguration = HostConfiguration.Build(args);
            var connectionString = hostConfiguration.Configuration.GetConnectionString("VehicleKhatabookDb");

            var optionsBuilder = new DbContextOptionsBuilder<VehicleKhatabookDbContext>();

            bool logSqlStatements = hostConfiguration.Configuration.GetValue<bool>("LogSqlStatements");

            if (logSqlStatements)
            {
                optionsBuilder.UseLoggerFactory(hostConfiguration.LoggerFactory);
            }

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("VehicleKhatabook.SchemaBuilder"));

            var dbContext = new VehicleKhatabookDbContext(optionsBuilder.Options);
            return dbContext;
        }
    }
}
