using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Identity.Application.Abstractions.Security;
using HRMS.Modules.Identity.Application.Features.Identity.BusinessRules;
using HRMS.Modules.Identity.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateUser;

public class CreateUserCommandHandler  : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IWriteRepository< HRMS.Modules.Identity.Domain.Entities.User, Guid> _userWriteRepository;
    private readonly IdentityBusinessRules _identityBusinessRules;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IWriteRepository< HRMS.Modules.Identity.Domain.Entities.User, Guid> userWriteRepository,
        IdentityBusinessRules identityBusinessRules,
        IPasswordHasher passwordHasher)
    {
        _userWriteRepository = userWriteRepository;
        _identityBusinessRules = identityBusinessRules;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        await _identityBusinessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _identityBusinessRules.EnsureEmployeeDoesNotHaveUserAsync(
            request.EmployeeId,
            cancellationToken);

        await _identityBusinessRules.EnsureUserNameUniqueAsync(
            request.UserName,
            cancellationToken);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new  HRMS.Modules.Identity.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            IsActive = true
        };

        await _userWriteRepository.AddAsync(
            user,
            cancellationToken);

        return user.Id;
    }
}