using HRMS.API.Services;
using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.API.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Employee.Application.DependencyInjection;
using HRMS.Modules.Department.Application.DependencyInjection;
using HRMS.Modules.Identity.Application.DependencyInjection;
using HRMS.Modules.Identity.Infrastructure.DependencyInjection;
using HRMS.Persistence.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CurrentUserContext>();
builder.Services.AddScoped<IUserContext>(
    sp => sp.GetRequiredService<CurrentUserContext>());
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddPersistence(builder.Configuration);
 //builder.Services.AddApplication();
// builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTransient( typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));



// Module Application registrations
builder.Services.AddEmployeeApplication();
builder.Services.AddDepartmentApplication();
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Identity Infrastructure registrations
//builder.Services.AddIdentityInfrastructure();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtOptions>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();



app.MapGet("/", () => "Hello World!");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
