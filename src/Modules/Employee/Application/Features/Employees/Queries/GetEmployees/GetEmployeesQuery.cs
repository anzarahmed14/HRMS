using HRMS.Application.Features.Employees.DTOs;
using MediatR;

namespace HRMS.Modules.Employee.Application.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>;