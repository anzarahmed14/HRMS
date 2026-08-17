using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.Infrastructure.Identity.Authorization;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserContext _userContext;
    private readonly IReadRepository<User, Guid> _userRepository;
    private readonly IReadRepository<Role, Guid> _roleRepository;
    private readonly IReadRepository<Permission, Guid> _permissionRepository;

    public PermissionAuthorizationHandler(
        IUserContext userContext,
        IReadRepository<User, Guid> userRepository,
        IReadRepository<Role, Guid> roleRepository,
        IReadRepository<Permission, Guid> permissionRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (!_userContext.UserId.HasValue)
        {
            return;
        }

        var user = await _userRepository.FirstOrDefaultAsync(
            x => x.Id == _userContext.UserId.Value,
            CancellationToken.None,
            x => x.UserRoles);

        if (user is null)
        {
            return;
        }

        var roleIds = user.UserRoles
            .Select(x => x.RoleId)
            .ToList();

        if (roleIds.Count == 0)
        {
            return;
        }

        var roles = await _roleRepository.FindAsync(
            x => roleIds.Contains(x.Id) && x.IsActive,
            CancellationToken.None,
            x => x.RolePermissions);

        var permissionIds = roles
            .SelectMany(x => x.RolePermissions)
            .Select(x => x.PermissionId)
            .Distinct()
            .ToList();

        if (permissionIds.Count == 0)
        {
            return;
        }

        var permissions = await _permissionRepository.FindAsync(
            x => permissionIds.Contains(x.Id) && x.IsActive,
            CancellationToken.None);

        var hasPermission = permissions.Any(
            x => x.Name == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}