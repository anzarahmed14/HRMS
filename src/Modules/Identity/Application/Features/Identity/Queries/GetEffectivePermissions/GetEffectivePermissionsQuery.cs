using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetEffectivePermissions;

public record GetEffectivePermissionsQuery(Guid UserId)
    : IRequest<EffectivePermissionsDto>;
