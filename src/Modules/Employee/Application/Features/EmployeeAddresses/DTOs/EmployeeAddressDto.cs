namespace HRMS.Application.Features.EmployeeAddresses.DTOs;

public class EmployeeAddressDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid AddressTypeId { get; set; }

    public Guid CountryId { get; set; }

    public Guid StateId { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
