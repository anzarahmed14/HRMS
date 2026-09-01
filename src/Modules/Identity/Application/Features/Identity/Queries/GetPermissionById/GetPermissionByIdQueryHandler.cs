using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissionById;

public class GetPermissionByIdQueryHandler
    : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
{
    private readonly IReadRepository<Permission, Guid> _repository;

    public GetPermissionByIdQueryHandler(
        IReadRepository<Permission, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PermissionDto?> Handle(
        GetPermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var permission = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (permission is null)
        {
            return null;
        }

        return new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            IsActive = permission.IsActive
        };
    }
}
