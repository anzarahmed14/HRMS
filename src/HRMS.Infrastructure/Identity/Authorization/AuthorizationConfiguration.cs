using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Infrastructure.Identity.Authorization;

public static class AuthorizationConfiguration
{
    public static IServiceCollection AddPermissionAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Employee.View", policy =>
            {
                policy.Requirements.Add(
                    new PermissionRequirement("Employee.View"));
            });

            options.AddPolicy("Employee.Create", policy =>
            {
                policy.Requirements.Add(
                    new PermissionRequirement("Employee.Create"));
            });

            options.AddPolicy("Employee.Update", policy =>
            {
                policy.Requirements.Add(
                    new PermissionRequirement("Employee.Update"));
            });

            options.AddPolicy("Employee.Delete", policy =>
            {
                policy.Requirements.Add(
                    new PermissionRequirement("Employee.Delete"));
            });
        });

        return services;
    }
}