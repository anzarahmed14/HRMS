using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Identity.BusinessRules;
using HRMS.Application.Features.Identity.DTOs;
using HRMS.Shared.Exceptions;
using MediatR;

namespace HRMS.Application.Features.Identity.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IdentityBusinessRules _identityBusinessRules;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IdentityBusinessRules identityBusinessRules,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _identityBusinessRules = identityBusinessRules;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _identityBusinessRules.EnsureUserCanLoginAsync(
            request.UserName,
            cancellationToken);

        var passwordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedException(
                "Invalid username or password.");
        }

        var accessToken = _jwtTokenService.GenerateToken(
            user.Id,
            user.EmployeeId,
            user.UserName);

        return new LoginResponseDto
        {
            UserId = user.Id,
            EmployeeId = user.EmployeeId,
            UserName = user.UserName,
            AccessToken = accessToken
        };
    }
}