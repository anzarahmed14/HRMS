using FluentValidation;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Identity.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(
        this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddScoped<IdentityBusinessRules>();
         services.AddScoped<PermissionBusinessRules>();
        return services;
    }
}