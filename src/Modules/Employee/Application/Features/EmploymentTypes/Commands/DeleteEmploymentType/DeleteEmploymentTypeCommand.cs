using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Commands.DeleteEmploymentType;

public record DeleteEmploymentTypeCommand(Guid Id) : IRequest;
