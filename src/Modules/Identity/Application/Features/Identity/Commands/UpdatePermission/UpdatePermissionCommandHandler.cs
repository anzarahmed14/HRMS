using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler
    : IRequestHandler<UpdatePermissionCommand>
{
    private readonly IWriteRepository<Permission, Guid> _writeRepository;
    private readonly PermissionBusinessRules _rules;

    public UpdatePermissionCommandHandler(
        IWriteRepository<Permission, Guid> writeRepository,
        PermissionBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _rules.EnsurePermissionExistsAsync(
            request.Id,
            cancellationToken);

        await _rules.EnsurePermissionNameUniqueAsync(
            request.Name,
            request.Id,
            cancellationToken);

        permission.Name = request.Name;
        permission.Description = request.Description;
        permission.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            permission,
            cancellationToken);
    }
}
