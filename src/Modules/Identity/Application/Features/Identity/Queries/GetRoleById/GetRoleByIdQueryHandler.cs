using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoleById;

public class GetRoleByIdQueryHandler
    : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IReadRepository<Role, Guid> _repository;

    public GetRoleByIdQueryHandler(
        IReadRepository<Role, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto?> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (role is null)
        {
            return null;
        }

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive
        };
    }
}
