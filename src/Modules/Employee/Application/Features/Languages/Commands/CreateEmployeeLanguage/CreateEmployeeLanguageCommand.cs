using MediatR;

namespace HRMS.Application.Features.Languages.Commands.CreateEmployeeLanguage;

public record CreateEmployeeLanguageCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; init; }

    public Guid LanguageId { get; init; }

    public string ProficiencyLevel { get; init; } = string.Empty;

    public bool CanRead { get; init; }

    public bool CanWrite { get; init; }

    public bool CanSpeak { get; init; }
}
