using FluentValidation;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.BusinessRules;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.BusinessRules;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.BusinessRules;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.BusinessRules;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.BusinessRules;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.BusinessRules;
using HRMS.Modules.Leave.Application.Features.LeaveYears.BusinessRules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Leave.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLeaveApplication(
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

        services.AddScoped<LeaveYearBusinessRules>();
        services.AddScoped<LeaveTypeBusinessRules>();
        services.AddScoped<LeavePolicyBusinessRules>();
        services.AddScoped<LeavePolicyRuleBusinessRules>();
        services.AddScoped<EmployeeLeaveEntitlementBusinessRules>();
        services.AddScoped<LeaveRequestBusinessRules>();
        services.AddScoped<CompanyHolidayBusinessRules>();
        return services;
    }
}