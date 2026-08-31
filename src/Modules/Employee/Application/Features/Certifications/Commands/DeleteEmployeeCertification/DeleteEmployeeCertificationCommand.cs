using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.DeleteEmployeeCertification;

public record DeleteEmployeeCertificationCommand(Guid Id) : IRequest;
