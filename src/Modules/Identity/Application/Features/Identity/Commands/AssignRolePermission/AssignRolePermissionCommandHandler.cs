using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignRolePermission;

public class AssignRolePermissionCommandHandler
    : IRequestHandler<AssignRolePermissionCommand, Guid>
{
    private readonly IReadRepository<RolePermission, Guid> _readRepository;
    private readonly IWriteRepository<RolePermission, Guid> _writeRepository;
    private readonly IReadRepository<Role, Guid> _roleRepository;
    private readonly IReadRepository<Permission, Guid> _permissionRepository;

    public AssignRolePermissionCommandHandler(
        IReadRepository<RolePermission, Guid> readRepository,
        IWriteRepository<RolePermission, Guid> writeRepository,
        IReadRepository<Role, Guid> roleRepository,
        IReadRepository<Permission, Guid> permissionRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<Guid> Handle(
        AssignRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(
            request.RoleId,
            cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(
                "Role",
                request.RoleId);
        }

        var permission = await _permissionRepository.GetByIdAsync(
            request.PermissionId,
            cancellationToken);

        if (permission is null)
        {
            throw new NotFoundException(
                "Permission",
                request.PermissionId);
        }

        var exists = await _readRepository.AnyAsync(
            x => x.RoleId == request.RoleId &&
                 x.PermissionId == request.PermissionId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This permission is already assigned to the role.");
        }

        var rolePermission = new RolePermission
        {
            Id = Guid.NewGuid(),
            RoleId = request.RoleId,
            PermissionId = request.PermissionId
        };

        await _writeRepository.AddAsync(
            rolePermission,
            cancellationToken);

        return rolePermission.Id;
    }
}
