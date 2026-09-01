using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserRoles;

public class GetUserRolesQueryHandler
    : IRequestHandler<GetUserRolesQuery, IEnumerable<UserRoleDto>>
{
    private readonly IReadRepository<UserRole, Guid> _repository;

    public GetUserRolesQueryHandler(
        IReadRepository<UserRole, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserRoleDto>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var userRoles = await _repository.FindAsync(
            x => x.UserId == request.UserId,
            cancellationToken,
            x => x.Role);

        return userRoles.Select(x => new UserRoleDto
        {
            Id = x.Id,
            UserId = x.UserId,
            RoleId = x.RoleId,
            RoleName = x.Role.Name,
            RoleDescription = x.Role.Description
        });
    }
}
