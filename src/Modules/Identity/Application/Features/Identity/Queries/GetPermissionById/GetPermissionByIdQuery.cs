using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id)
    : IRequest<PermissionDto?>;
