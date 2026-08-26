using HRMS.Application.Features.EmployeeAddresses.DTOs;
using MediatR;

namespace HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddressById;

public record GetEmployeeAddressByIdQuery(Guid Id)
    : IRequest<EmployeeAddressDto?>;
