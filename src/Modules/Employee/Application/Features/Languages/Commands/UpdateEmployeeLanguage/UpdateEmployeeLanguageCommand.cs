using MediatR;

namespace HRMS.Application.Features.Languages.Commands.UpdateEmployeeLanguage;

public record UpdateEmployeeLanguageCommand : IRequest
{
    public Guid Id { get; init; }

    public Guid EmployeeId { get; init; }

    public Guid LanguageId { get; init; }

    public string ProficiencyLevel { get; init; } = string.Empty;

    public bool CanRead { get; init; }

    public bool CanWrite { get; init; }

    public bool CanSpeak { get; init; }

    public bool IsActive { get; init; } = true;
}
