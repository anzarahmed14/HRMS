using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoles;

public class GetRolesQueryHandler
    : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IReadRepository<Role, Guid> _repository;

    public GetRolesQueryHandler(
        IReadRepository<Role, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoleDto>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _repository.GetAllAsync(
            cancellationToken);

        return roles.Select(x => new RoleDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive
        });
    }
}
