using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateUser;

public class DeactivateUserCommandHandler
    : IRequestHandler<DeactivateUserCommand>
{
    private readonly IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _readRepository;
    private readonly IWriteRepository<Domain.Entities.User, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;

    public DeactivateUserCommandHandler(
        IReadRepository<Domain.Entities.User, Guid> readRepository,
        IWriteRepository<Domain.Entities.User, Guid> writeRepository,
        IdentityBusinessRules rules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        DeactivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _rules.EnsureUserExistsAsync(
            request.Id,
            cancellationToken);

        user.IsActive = false;

        await _writeRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}
