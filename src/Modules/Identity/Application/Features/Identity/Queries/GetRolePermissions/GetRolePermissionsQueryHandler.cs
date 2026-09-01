using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRolePermissions;

public class GetRolePermissionsQueryHandler
    : IRequestHandler<GetRolePermissionsQuery, IEnumerable<RolePermissionDto>>
{
    private readonly IReadRepository<RolePermission, Guid> _repository;

    public GetRolePermissionsQueryHandler(
        IReadRepository<RolePermission, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RolePermissionDto>> Handle(
        GetRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var rolePermissions = await _repository.FindAsync(
            x => x.RoleId == request.RoleId,
            cancellationToken,
            x => x.Permission);

        return rolePermissions.Select(x => new RolePermissionDto
        {
            Id = x.Id,
            RoleId = x.RoleId,
            PermissionId = x.PermissionId,
            PermissionName = x.Permission.Name,
            PermissionDescription = x.Permission.Description
        });
    }
}
