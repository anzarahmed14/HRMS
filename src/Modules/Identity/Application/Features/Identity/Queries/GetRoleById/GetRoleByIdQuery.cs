using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id)
    : IRequest<RoleDto?>;
