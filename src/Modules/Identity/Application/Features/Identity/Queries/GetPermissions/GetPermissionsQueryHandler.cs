using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissions;

public class GetPermissionsQueryHandler
    : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IReadRepository<Permission, Guid> _repository;

    public GetPermissionsQueryHandler(
        IReadRepository<Permission, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(
        GetPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _repository.GetAllAsync(
            cancellationToken);

        return permissions.Select(x => new PermissionDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            IsActive = x.IsActive
        });
    }
}
