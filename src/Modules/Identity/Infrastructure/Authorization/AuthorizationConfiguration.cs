using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
namespace HRMS.Modules.Identity.Infrastructure.Authorization;

/// <summary>
/// Wires up the generic, database-driven permission authorization
/// infrastructure. Individual permissions are never registered here —
/// <see cref="PermissionAuthorizationPolicyProvider"/> resolves a policy
/// for any permission declared via <c>PermissionAuthorizeAttribute</c> on
/// demand, and <see cref="PermissionAuthorizationHandler"/> makes the
/// actual authorization decision from the User → UserRole → Role →
/// RolePermission → Permission relationships.
/// </summary>
public static class AuthorizationConfiguration
{
    public static IServiceCollection AddIdentityAuthorization( this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddScoped<
            IAuthorizationHandler,
            PermissionAuthorizationHandler>();

        return services;
    }
}
