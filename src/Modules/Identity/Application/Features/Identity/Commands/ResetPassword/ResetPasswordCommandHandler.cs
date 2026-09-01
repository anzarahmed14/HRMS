using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.ResetPassword;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand>
{
    private readonly IWriteRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        IWriteRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> writeRepository,
        IdentityBusinessRules rules,
        IPasswordHasher passwordHasher)
    {
        _writeRepository = writeRepository;
        _rules = rules;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _rules.EnsureUserExistsAsync(
            request.UserId,
            cancellationToken);

        user.PasswordHash = _passwordHasher.Hash(
            request.NewPassword);

        await _writeRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}