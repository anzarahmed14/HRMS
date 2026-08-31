using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.VerifyEmployeeCertification;

public record VerifyEmployeeCertificationCommand(Guid Id) : IRequest;
