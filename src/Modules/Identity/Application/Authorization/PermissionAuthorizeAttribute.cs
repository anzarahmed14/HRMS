using Microsoft.AspNetCore.Authorization;

namespace HRMS.Modules.Identity.Application.Authorization;

/// <summary>
/// Declares that an endpoint requires a specific permission, expressed as
/// one of the codes in <see cref="PermissionNames"/>.
/// </summary>
/// <remarks>
/// This attribute only declares the required permission — it performs no
/// authorization decision itself and never touches the database. The
/// permission name is encoded into the underlying ASP.NET Core
/// <see cref="AuthorizeAttribute.Policy"/> using <see cref="PolicyPrefix"/>
/// so that a generic authorization policy provider can recognize the
/// request and build an <c>AuthorizationPolicy</c> containing a
/// <c>PermissionRequirement</c> for it on the fly, without requiring the
/// permission to be pre-registered anywhere. The actual authorization
/// decision is made by <c>PermissionAuthorizationHandler</c> against the
/// database-driven User → UserRole → Role → RolePermission → Permission
/// relationships.
/// </remarks>
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Prefix used to mark a policy name as a permission request, so the
    /// generic policy provider can distinguish it from any other named
    /// authorization policy and fall back correctly when it isn't one.
    /// </summary>
    public const string PolicyPrefix = "Permission:";

    public string Permission { get; }

    public PermissionAuthorizeAttribute(string permission)
        : base(BuildPolicyName(permission))
    {
        Permission = permission;
    }

    private static string BuildPolicyName(string permission)
    {
        if (string.IsNullOrWhiteSpace(permission))
        {
            throw new ArgumentException(
                "Permission cannot be null or empty.",
                nameof(permission));
        }

        return PolicyPrefix + permission;
    }
}
