using FluentValidation;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Companies.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCompaniesApplication(
        this IServiceCollection services)
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

        services.AddScoped<CompanyBusinessRules>();

        return services;
    }
}