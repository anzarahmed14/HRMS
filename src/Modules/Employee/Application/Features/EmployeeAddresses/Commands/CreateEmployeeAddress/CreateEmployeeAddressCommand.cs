using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Commands.CreateEmployeeAddress;

public record CreateEmployeeAddressCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid AddressTypeId { get; init; }

    public Guid CountryId { get; init; }

    public Guid StateId { get; init; }

    public string AddressLine1 { get; init; } = string.Empty;

    public string? AddressLine2 { get; init; }

    public string City { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public bool IsPrimary { get; init; }
}
