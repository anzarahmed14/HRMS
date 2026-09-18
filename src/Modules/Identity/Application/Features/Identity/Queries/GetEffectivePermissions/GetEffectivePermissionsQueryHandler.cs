using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetEffectivePermissions;

public class GetEffectivePermissionsQueryHandler : IRequestHandler<GetEffectivePermissionsQuery, EffectivePermissionsDto>
{
    private readonly IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _userRepository;
    private readonly IReadRepository<Role, Guid> _roleRepository;
    private readonly IReadRepository<Permission, Guid> _permissionRepository;

    public GetEffectivePermissionsQueryHandler(
        IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> userRepository,
        IReadRepository<Role, Guid> roleRepository,
        IReadRepository<Permission, Guid> permissionRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<EffectivePermissionsDto> Handle(
        GetEffectivePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new EffectivePermissionsDto();

        var user = await _userRepository.FirstOrDefaultAsync(
            x => x.Id == request.UserId,
            cancellationToken,
            x => x.UserRoles);

        if (user is null)
        {
            return result;
        }

        var roleIds = user.UserRoles
            .Select(x => x.RoleId)
            .ToList();

        if (roleIds.Count == 0)
        {
            return result;
        }

        var roles = await _roleRepository.FindAsync(
            x => roleIds.Contains(x.Id) && x.IsActive,
            cancellationToken,
            x => x.RolePermissions);

        result.Roles = roles
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        var permissionIds = roles
            .SelectMany(x => x.RolePermissions)
            .Select(x => x.PermissionId)
            .Distinct()
            .ToList();

        if (permissionIds.Count == 0)
        {
            return result;
        }

        var permissions = await _permissionRepository.FindAsync(
            x => permissionIds.Contains(x.Id) && x.IsActive,
            cancellationToken);

        result.Permissions = permissions
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        return result;
    }
}
