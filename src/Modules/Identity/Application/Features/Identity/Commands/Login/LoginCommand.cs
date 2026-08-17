using MediatR;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.Login;

public record LoginCommand : IRequest<LoginResponseDto>
{
    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}