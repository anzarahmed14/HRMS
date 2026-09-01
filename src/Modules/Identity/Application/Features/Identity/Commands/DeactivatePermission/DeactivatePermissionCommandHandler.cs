using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivatePermission;

public class DeactivatePermissionCommandHandler
    : IRequestHandler<DeactivatePermissionCommand>
{
    private readonly IWriteRepository<Permission, Guid> _writeRepository;
    private readonly PermissionBusinessRules _rules;

    public DeactivatePermissionCommandHandler(
        IWriteRepository<Permission, Guid> writeRepository,
        PermissionBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        DeactivatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _rules.EnsurePermissionExistsAsync(
            request.Id,
            cancellationToken);

        permission.IsActive = false;

        await _writeRepository.UpdateAsync(
            permission,
            cancellationToken);
    }
}
