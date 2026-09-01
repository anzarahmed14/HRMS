using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateRole;

public class DeactivateRoleCommandHandler
    : IRequestHandler<DeactivateRoleCommand>
{
    private readonly IWriteRepository<Role, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;

    public DeactivateRoleCommandHandler(
        IWriteRepository<Role, Guid> writeRepository,
        IdentityBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        DeactivateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _rules.EnsureRoleExistsAsync(
            request.Id,
            cancellationToken);

        role.IsActive = false;

        await _writeRepository.UpdateAsync(
            role,
            cancellationToken);
    }
}
