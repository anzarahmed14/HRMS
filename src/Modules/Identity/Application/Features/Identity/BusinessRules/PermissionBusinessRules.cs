using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Domain.Entities;

namespace HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;

public class PermissionBusinessRules
{
    private readonly IReadRepository<Permission, Guid> _repository;
    private readonly IReadRepository<Module, Guid> _moduleRepository;

    public PermissionBusinessRules(
        IReadRepository<Permission, Guid> repository,
        IReadRepository<Module, Guid> moduleRepository)
    {
        _repository = repository;
        _moduleRepository = moduleRepository;
    }

    public async Task<Permission> EnsurePermissionExistsAsync(
        Guid permissionId,
        CancellationToken cancellationToken = default)
    {
        var permission = await _repository.GetByIdAsync(
            permissionId,
            cancellationToken);

        if (permission is null)
        {
            throw new NotFoundException(
                "Permission",
                permissionId);
        }

        return permission;
    }

    public async Task EnsurePermissionNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Name == name,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Permission name already exists.");
        }
    }

    public async Task EnsurePermissionNameUniqueAsync(
        string name,
        Guid excludePermissionId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _repository.AnyAsync(
            x => x.Name == name &&
                 x.Id != excludePermissionId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Permission name already exists.");
        }
    }

    public async Task EnsureModuleExistsAsync(
        Guid moduleId,
        CancellationToken cancellationToken = default)
    {
        var module = await _moduleRepository.GetByIdAsync(
            moduleId,
            cancellationToken);

        if (module is null)
        {
            throw new NotFoundException(
                "Module",
                moduleId);
        }
    }
}
