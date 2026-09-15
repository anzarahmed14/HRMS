using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.DeleteCertification;

public record DeleteCertificationCommand(
    Guid Id) : IRequest;
