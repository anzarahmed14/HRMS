using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Identity.BusinessRules;
using HRMS.Application.Features.Identity.DTOs;
using HRMS.Domain.Entities;
using HRMS.Domain.Interfaces;
using HRMS.Shared.Exceptions;
using MediatR;

namespace HRMS.Application.Features.Identity.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IdentityBusinessRules _identityBusinessRules;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IdentityBusinessRules identityBusinessRules,
        IPasswordHasher passwordHasher)
    {
        _identityBusinessRules = identityBusinessRules;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find user and make sure the account can login
        var user = await _identityBusinessRules.EnsureUserCanLoginAsync(
            request.UserName,
            cancellationToken);

        // 2. Verify password
        var passwordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedException(
                "Invalid username or password.");
        }

        // 3. Return authenticated user information
        return new LoginResponseDto
        {
            UserId = user.Id,
            EmployeeId = user.EmployeeId,
            UserName = user.UserName
        };
    }
}