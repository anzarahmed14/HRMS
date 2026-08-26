using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.UpdateEmployeeContact;

public record UpdateEmployeeContactCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public string ContactType { get; init; } = string.Empty;

    public string ContactValue { get; init; } = string.Empty;

    public bool IsPrimary { get; init; }
}
