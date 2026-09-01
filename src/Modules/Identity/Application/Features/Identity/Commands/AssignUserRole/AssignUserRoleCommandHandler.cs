using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler
    : IRequestHandler<AssignUserRoleCommand, Guid>
{
    private readonly IReadRepository<UserRole, Guid> _readRepository;
    private readonly IWriteRepository<UserRole, Guid> _writeRepository;
    private readonly IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _userRepository;
    private readonly IReadRepository<Role, Guid> _roleRepository;

    public AssignUserRoleCommandHandler(
        IReadRepository<UserRole, Guid> readRepository,
        IWriteRepository<UserRole, Guid> writeRepository,
        IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> userRepository,
        IReadRepository<Role, Guid> roleRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Guid> Handle(
        AssignUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User",
                request.UserId);
        }

        var role = await _roleRepository.GetByIdAsync(
            request.RoleId,
            cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(
                "Role",
                request.RoleId);
        }

        var exists = await _readRepository.AnyAsync(
            x => x.UserId == request.UserId &&
                 x.RoleId == request.RoleId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "This role is already assigned to the user.");
        }

        var userRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        await _writeRepository.AddAsync(
            userRole,
            cancellationToken);

        return userRole.Id;
    }
}
