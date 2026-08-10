using MediatR;

namespace HRMS.Application.Features.Identity.Commands.CreateUser;

public record CreateUserCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}