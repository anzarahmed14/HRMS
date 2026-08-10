using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Identity.BusinessRules;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Identity.Commands.CreateUser;

public class CreateUserCommandHandler  : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IWriteRepository<User, Guid> _userWriteRepository;
    private readonly IdentityBusinessRules _identityBusinessRules;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IWriteRepository<User, Guid> userWriteRepository,
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
        // 1. Employee must exist
        await _identityBusinessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        // 2. Employee must not already have a User
        await _identityBusinessRules.EnsureEmployeeDoesNotHaveUserAsync(
            request.EmployeeId,
            cancellationToken);

        // 3. Username must be unique
        await _identityBusinessRules.EnsureUserNameUniqueAsync(
            request.UserName,
            cancellationToken);

        // 4. Hash password
        var passwordHash = _passwordHasher.Hash(request.Password);

        // 5. Create User
        var user = new User
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            IsActive = true
        };

        // 6. Save User
        await _userWriteRepository.AddAsync(
            user,
            cancellationToken);

        return user.Id;
    }
}