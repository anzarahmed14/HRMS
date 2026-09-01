using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateRole;

public class UpdateRoleCommandHandler
    : IRequestHandler<UpdateRoleCommand>
{
    private readonly IWriteRepository<Role, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;

    public UpdateRoleCommandHandler(
        IWriteRepository<Role, Guid> writeRepository,
        IdentityBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _rules.EnsureRoleExistsAsync(
            request.Id,
            cancellationToken);

        await _rules.EnsureRoleNameUniqueAsync(
            request.Name,
            request.Id,
            cancellationToken);

        role.Name = request.Name;
        role.Description = request.Description;
        role.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            role,
            cancellationToken);
    }
}
