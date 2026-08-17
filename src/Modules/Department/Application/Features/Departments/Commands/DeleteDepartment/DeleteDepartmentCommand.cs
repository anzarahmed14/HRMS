using MediatR;

namespace HRMS.Modules.Department.Application.Features.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(Guid Id) : IRequest;