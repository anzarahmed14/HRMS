using HRMS.Application.Features.Identity.DTOs;
using MediatR;

namespace HRMS.Application.Features.Identity.Commands.Login;

public record LoginCommand : IRequest<LoginResponseDto>
{
    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}