using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateRole;

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IWriteRepository<Role, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;

    public CreateRoleCommandHandler(
        IWriteRepository<Role, Guid> writeRepository,
        IdentityBusinessRules rules)
    {
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task<Guid> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        await _rules.EnsureRoleNameUniqueAsync(
            request.Name,
            cancellationToken);

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            role,
            cancellationToken);

        return role.Id;
    }
}
