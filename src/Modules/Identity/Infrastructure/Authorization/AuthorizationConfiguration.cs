using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
namespace HRMS.Modules.Identity.Infrastructure.Authorization;

public static class AuthorizationConfiguration
{
    public static IServiceCollection AddIdentityAuthorization( this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "User.ResetPassword",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("User.ResetPassword")));

            options.AddPolicy(
                "Employee.View",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Employee.View")));

            options.AddPolicy(
                "Employee.Create",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Employee.Create")));

            options.AddPolicy(
                "Employee.Update",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Employee.Update")));

            options.AddPolicy(
                "Employee.Delete",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Employee.Delete")));

            options.AddPolicy(
                "Department.View",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Department.View")));

            options.AddPolicy(
                "Department.Create",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Department.Create")));

            options.AddPolicy(
                "Department.Update",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Department.Update")));

            options.AddPolicy(
                "Department.Delete",
                policy =>
                    policy.Requirements.Add(
                        new PermissionRequirement("Department.Delete")));
        });

        services.AddScoped<
            IAuthorizationHandler,
            PermissionAuthorizationHandler>();

        return services;
    }
}