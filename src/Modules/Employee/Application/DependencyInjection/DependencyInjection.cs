using FluentValidation;
using HRMS.Application.Features.BankAccounts.BusinessRules;
using HRMS.Application.Features.EmergencyContacts.BusinessRules;
using HRMS.Application.Features.EmployeeAddresses.BusinessRules;
using HRMS.Application.Features.EmployeeContacts.BusinessRules;
using HRMS.Application.Features.Employees.BusinessRules;
using HRMS.Application.Features.EmploymentStatuses.BusinessRules;
using HRMS.Application.Features.EmploymentTypes.BusinessRules;
using HRMS.Application.Features.GovernmentIdentifiers.BusinessRules;
using HRMS.BuildingBlocks.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Employee.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEmployeeApplication(
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

        services.AddScoped<EmployeeBusinessRules>();
        services.AddScoped<EmploymentTypeBusinessRules>();
        services.AddScoped<EmploymentStatusBusinessRules>();
        services.AddScoped<EmployeeAddressBusinessRules>();
        services.AddScoped<EmployeeContactBusinessRules>();
        services.AddScoped<EmergencyContactBusinessRules>();
        services.AddScoped<BankAccountBusinessRules>();
        services.AddScoped<EmployeeGovernmentIdentifierBusinessRules>();

        return services;
    }
}
