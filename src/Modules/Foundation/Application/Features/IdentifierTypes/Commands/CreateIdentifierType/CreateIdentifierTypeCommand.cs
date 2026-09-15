using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.CreateIdentifierType;

public record CreateIdentifierTypeCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsSensitive { get; init; }
    public bool IsActive { get; init; } = true;
}
