using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.DeleteIdentifierType;

public record DeleteIdentifierTypeCommand(
    Guid Id) : IRequest;
