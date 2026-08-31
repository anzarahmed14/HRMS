using MediatR;

namespace HRMS.Application.Features.Educations.Commands.DeleteEmployeeEducation;

public record DeleteEmployeeEducationCommand(Guid Id) : IRequest;
