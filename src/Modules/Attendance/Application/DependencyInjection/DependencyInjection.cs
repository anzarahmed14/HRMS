using FluentValidation;
using HRMS.BuildingBlocks.Application.Behaviors;
using HRMS.Modules.Attendance.Application.Features.AttendanceDevices.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.BusinessRules;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.BusinessRules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRMS.Modules.Attendance.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddAttendanceApplication(
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

        services.AddScoped<AttendancePolicyBusinessRules>();
        services.AddScoped<AttendanceShiftBusinessRules>();
        services.AddScoped<EmployeeShiftAssignmentBusinessRules>();
        services.AddScoped<AttendanceSourceBusinessRules>();
        services.AddScoped<AttendanceDeviceBusinessRules>();
        services.AddScoped<AttendanceRawLogBusinessRules>();
        services.AddScoped<AttendanceCalendarBusinessRules>();
        
        return services;
    }
}




