using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;
