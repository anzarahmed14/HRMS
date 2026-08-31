using MediatR;

namespace HRMS.Application.Features.Languages.Commands.DeleteEmployeeLanguage;

public record DeleteEmployeeLanguageCommand(Guid Id) : IRequest;
