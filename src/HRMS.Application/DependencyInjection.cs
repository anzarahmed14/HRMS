using FluentValidation;
using HRMS.Application.Common.Behaviors;
using HRMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

       services.AddAutoMapper(cfg => 
        {
            cfg.AddMaps(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        // services.AddScoped<EmployeeBusinessRules>();    
        // services.AddScoped<DepartmentBusinessRules>();    
        // services.AddScoped<IdentityBusinessRules>();  
        // services.AddScoped<IPasswordHasher, PasswordHasher>();  

        return services;
    }
}