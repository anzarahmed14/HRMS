using HRMS.API.Middleware;
using HRMS.API.Services;
using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Attendance.Application.DependencyInjection;
using HRMS.Modules.Attendance.Application.Services;
using HRMS.Modules.Attendance.Infrastructure.DependencyInjection;
using HRMS.Modules.Companies.Application.DependencyInjection;
using HRMS.Modules.Department.Application.DependencyInjection;
using HRMS.Modules.Employee.Application.DependencyInjection;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Identity.Application.DependencyInjection;
using HRMS.Modules.Identity.Infrastructure.DependencyInjection;
using HRMS.Modules.Leave.Application.DependencyInjection;
using HRMS.Modules.Leave.Infrastructure.DependencyInjection;
using HRMS.Persistence.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

builder.Services.AddScoped<IAttendanceCalculationService,AttendanceCalculationService>();

builder.Services.AddScoped<IAttendanceDayStatusService, AttendanceDayStatusService>();



builder.Services.AddEmployeeApplication();
builder.Services.AddDepartmentApplication();
builder.Services.AddIdentityApplication();
builder.Services.AddCompaniesApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddLeaveApplication();
builder.Services.AddLeaveInfrastructure();
builder.Services.AddAttendanceApplication();
builder.Services.AddAttendanceInfrastructure();

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

