using HRMS.Application.Features.Departments.DTOs;
using MediatR;
namespace HRMS.Modules.Department.Application.Features.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery
    : IRequest<IReadOnlyList<DepartmentDto>>;