using log4net.Config;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Extensions;
using VehicleKhatabook.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using VehicleKhatabook.Services.Services;
using VehicleKhatabook.Models.Common;

var baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
var configuration = new ConfigurationBuilder()
   .AddJsonFile($"{baseDir}//appsettings.json")
   .Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableBoolConverter());
    });


builder.Services.AddDbContext<VehicleKhatabookDbContext>(options =>
options.UseSqlServer(
                builder.Configuration.GetConnectionString("VehicleKhatabookDb"),
                b => b.MigrationsAssembly("VehicleKhatabook.SchemaBuilder")
            )
        );
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerPolicy", policy => policy.RequireRole("owner"));
    options.AddPolicy("DriverPolicy", policy => policy.RequireRole("driver"));
    options.AddPolicy("OwnerOrDriverPolicy", policy => policy.RequireRole("owner", "driver"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAllMinimalApiDefinitions(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vechilekhatabook Api",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

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
    app.UseExceptionHandlerMiddleware();

}
else
{
    app.UseExceptionHandlerMiddleware();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpointDefinitions();
app.UseExceptionHandlerMiddleware();
app.MapControllers();

app.Run();
