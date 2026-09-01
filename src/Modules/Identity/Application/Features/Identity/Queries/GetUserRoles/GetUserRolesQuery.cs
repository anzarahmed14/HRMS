using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserRoles;

public record GetUserRolesQuery(Guid UserId)
    : IRequest<IEnumerable<UserRoleDto>>;
