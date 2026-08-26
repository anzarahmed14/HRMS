using MediatR;

namespace HRMS.Application.Features.EmployeeContacts.Commands.CreateEmployeeContact;

public record CreateEmployeeContactCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public string ContactType { get; init; } = string.Empty;

    public string ContactValue { get; init; } = string.Empty;

    public bool IsPrimary { get; init; }
}
