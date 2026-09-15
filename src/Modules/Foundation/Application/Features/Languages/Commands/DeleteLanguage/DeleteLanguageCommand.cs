using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Commands.DeleteLanguage;

public record DeleteLanguageCommand(
    Guid Id) : IRequest;
