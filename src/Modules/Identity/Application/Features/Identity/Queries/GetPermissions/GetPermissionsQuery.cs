using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>;
