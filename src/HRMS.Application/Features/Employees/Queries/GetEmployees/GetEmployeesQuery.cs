using HRMS.Application.Features.Employees.DTOs;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>;