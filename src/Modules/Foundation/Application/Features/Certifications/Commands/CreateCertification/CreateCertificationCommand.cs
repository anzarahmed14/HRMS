using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Certifications.Commands.CreateCertification;

public record CreateCertificationCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? IssuingOrganization { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
}
