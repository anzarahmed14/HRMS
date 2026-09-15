using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.CreateLanguage;

public record CreateLanguageCommand : IRequest<Guid>
{
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; } = true;
}
