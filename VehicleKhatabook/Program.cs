using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Extensions;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseEndpointDefinitions();
app.UseAuthorization();

app.MapControllers();

app.Run();
