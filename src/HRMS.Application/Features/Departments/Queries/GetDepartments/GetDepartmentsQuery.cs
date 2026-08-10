using HRMS.Application.Features.Departments.DTOs;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery
    : IRequest<IReadOnlyList<DepartmentDto>>;