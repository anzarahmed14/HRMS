using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.ResetPassword;

public record ResetPasswordCommand : IRequest
{
    public Guid UserId { get; init; }

    public string NewPassword { get; init; } = string.Empty;
}