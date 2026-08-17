using HRMS.Application.Features.Departments.DTOs;
using MediatR;

namespace HRMS.Modules.Department.Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(Guid Id)
    : IRequest<DepartmentDto?>;