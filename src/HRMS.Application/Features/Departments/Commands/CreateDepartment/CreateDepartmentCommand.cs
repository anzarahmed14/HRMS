using MediatR;
namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;
public record CreateDepartmentCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}