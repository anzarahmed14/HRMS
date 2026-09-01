using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveUserRole;

public class RemoveUserRoleCommandHandler
    : IRequestHandler<RemoveUserRoleCommand>
{
    private readonly IReadRepository<UserRole, Guid> _readRepository;
    private readonly IWriteRepository<UserRole, Guid> _writeRepository;

    public RemoveUserRoleCommandHandler(
        IReadRepository<UserRole, Guid> readRepository,
        IWriteRepository<UserRole, Guid> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task Handle(
        RemoveUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var userRole = await _readRepository.FirstOrDefaultAsync(
            x => x.UserId == request.UserId &&
                 x.RoleId == request.RoleId,
            cancellationToken);

        if (userRole is null)
        {
            throw new NotFoundException(
                "UserRole",
                $"{request.UserId}:{request.RoleId}");
        }

        await _writeRepository.DeleteAsync(
            userRole,
            cancellationToken);
    }
}
