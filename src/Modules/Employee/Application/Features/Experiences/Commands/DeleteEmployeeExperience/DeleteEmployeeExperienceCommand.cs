using MediatR;

namespace HRMS.Application.Features.Experiences.Commands.DeleteEmployeeExperience;

public record DeleteEmployeeExperienceCommand(Guid Id) : IRequest;
