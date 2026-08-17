using HRMS.Application.Features.Employees.DTOs;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDto?>;