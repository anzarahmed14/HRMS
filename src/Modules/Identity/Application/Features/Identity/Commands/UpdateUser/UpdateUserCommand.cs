using MediatR;

namespace HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateUser;

public record UpdateUserCommand : IRequest
{
    public Guid Id { get; init; }

    public string UserName { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
