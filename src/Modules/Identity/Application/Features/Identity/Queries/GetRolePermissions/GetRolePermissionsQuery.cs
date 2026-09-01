using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRolePermissions;

public record GetRolePermissionsQuery(Guid RoleId)
    : IRequest<IEnumerable<RolePermissionDto>>;
