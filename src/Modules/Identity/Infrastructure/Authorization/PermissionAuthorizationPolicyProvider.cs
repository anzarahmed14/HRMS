using HRMS.Modules.Identity.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HRMS.Modules.Identity.Infrastructure.Authorization;

/// <summary>
/// Resolves an <see cref="AuthorizationPolicy"/> for any permission
/// declared through <see cref="PermissionAuthorizeAttribute"/>, without
/// requiring the permission to be individually registered with
/// <c>AddPolicy</c>. Any policy name that isn't a permission request is
/// delegated to the default ASP.NET Core policy provider unchanged, so
/// plain <c>[Authorize]</c>, authentication requirements, and any other
/// named policy keep working exactly as before.
/// </summary>
/// <remarks>
/// This provider performs no database access and has no knowledge of any
/// specific HRMS module or permission — it only recognizes the
/// <see cref="PermissionAuthorizeAttribute.PolicyPrefix"/> convention and
/// wraps the remaining permission name in a <see cref="PermissionRequirement"/>.
/// The actual authorization decision is made later by
/// <see cref="PermissionAuthorizationHandler"/>.
/// </remarks>
public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public PermissionAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(
                PermissionAuthorizeAttribute.PolicyPrefix,
                StringComparison.Ordinal))
        {
            var permission = policyName[PermissionAuthorizeAttribute.PolicyPrefix.Length..];

            if (string.IsNullOrWhiteSpace(permission))
            {
                throw new ArgumentException(
                    "Permission name in policy cannot be null or empty.",
                    nameof(policyName));
            }

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
