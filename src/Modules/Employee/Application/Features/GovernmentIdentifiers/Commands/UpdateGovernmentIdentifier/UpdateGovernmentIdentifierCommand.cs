using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.UpdateGovernmentIdentifier;

public record UpdateGovernmentIdentifierCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public Guid IdentifierTypeId { get; init; }

    public string IdentifierNumber { get; init; } = string.Empty;

    public DateOnly? IssueDate { get; init; }

    public DateOnly? ExpiryDate { get; init; }
}
