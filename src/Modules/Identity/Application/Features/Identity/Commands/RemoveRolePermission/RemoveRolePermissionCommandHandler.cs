using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveRolePermission;

public class RemoveRolePermissionCommandHandler
    : IRequestHandler<RemoveRolePermissionCommand>
{
    private readonly IReadRepository<RolePermission, Guid> _readRepository;
    private readonly IWriteRepository<RolePermission, Guid> _writeRepository;

    public RemoveRolePermissionCommandHandler(
        IReadRepository<RolePermission, Guid> readRepository,
        IWriteRepository<RolePermission, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        RemoveRolePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var rolePermission = await _readRepository.FirstOrDefaultAsync(
            x => x.RoleId == request.RoleId &&
                 x.PermissionId == request.PermissionId,
            cancellationToken);

        if (rolePermission is null)
        {
            throw new NotFoundException(
                "RolePermission",
                $"{request.RoleId}:{request.PermissionId}");
        }

        await _writeRepository.DeleteAsync(
            rolePermission,
            cancellationToken);
    }
}
