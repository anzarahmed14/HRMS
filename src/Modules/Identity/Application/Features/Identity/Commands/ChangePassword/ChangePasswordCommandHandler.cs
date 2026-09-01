using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.ChangePassword;

public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand>
{
    private readonly IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _readRepository;
    private readonly IWriteRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> _writeRepository;
    private readonly IdentityBusinessRules _rules;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        IReadRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> readRepository,
        IWriteRepository<HRMS.Modules.Identity.Domain.Entities.User, Guid> writeRepository,
        IdentityBusinessRules rules,
        IPasswordHasher passwordHasher)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _rules = rules;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _rules.EnsureUserExistsAsync(
            request.UserId,
            cancellationToken);

        if (!user.IsActive)
        {
            throw new UnauthorizedException(
                "User account is inactive.");
        }

        var currentPasswordValid = _passwordHasher.Verify(
            request.CurrentPassword,
            user.PasswordHash);

        if (!currentPasswordValid)
        {
            throw new UnauthorizedException(
                "Current password is incorrect.");
        }

        user.PasswordHash = _passwordHasher.Hash(
            request.NewPassword);

        await _writeRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}
