using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreatePermission;

public class CreatePermissionCommandHandler
    : IRequestHandler<CreatePermissionCommand, Guid>
{
    private readonly IWriteRepository<Permission, Guid> _writeRepository;
    private readonly PermissionBusinessRules _rules;

    public CreatePermissionCommandHandler(
        IWriteRepository<Permission, Guid> writeRepository,
        PermissionBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task<Guid> Handle(
        CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        await _rules.EnsurePermissionNameUniqueAsync(
            request.Name,
            cancellationToken);

        var permission = new Permission
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            permission,
            cancellationToken);

        return permission.Id;
    }
}
