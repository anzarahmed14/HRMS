using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.DeleteGovernmentIdentifier;

public record DeleteGovernmentIdentifierCommand(Guid Id) : IRequest;
