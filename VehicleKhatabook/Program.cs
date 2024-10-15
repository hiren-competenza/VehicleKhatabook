using log4net.Config;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Extensions;
using VehicleKhatabook.Infrastructure;

var baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
var configuration = new ConfigurationBuilder()
   .AddJsonFile($"{baseDir}//appsettings.json")
   .Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddDbContext<VehicleKhatabookDbContext>(options =>
options.UseSqlServer(
                builder.Configuration.GetConnectionString("VehicleKhatabookDb"),
                b => b.MigrationsAssembly("VehicleKhatabook.SchemaBuilder")
            )
        );
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAllMinimalApiDefinitions(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandlerMiddleware();
}

app.UseHttpsRedirection();
app.UseEndpointDefinitions();
app.UseAuthorization();
app.UseExceptionHandlerMiddleware();
app.MapControllers();

app.Run();
