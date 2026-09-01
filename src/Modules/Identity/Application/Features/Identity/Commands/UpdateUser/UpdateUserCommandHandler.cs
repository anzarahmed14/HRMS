using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly IReadRepository<Domain.Entities.User, Guid> _readRepository;
    private readonly IWriteRepository<Domain.Entities.User, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;

    public UpdateUserCommandHandler(
        IReadRepository<Domain.Entities.User, Guid> readRepository,
        IWriteRepository<Domain.Entities.User, Guid> writeRepository,
        IdentityBusinessRules rules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _rules.EnsureUserExistsAsync(
            request.Id,
            cancellationToken);

        await _rules.EnsureUserNameUniqueAsync(
            request.UserName,
            request.Id,
            cancellationToken);

        user.UserName = request.UserName;
        user.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}
