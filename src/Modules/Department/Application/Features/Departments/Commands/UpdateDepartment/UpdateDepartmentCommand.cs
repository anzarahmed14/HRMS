
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}
