using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Commands.VerifyGovernmentIdentifier;

public record VerifyGovernmentIdentifierCommand(Guid Id) : IRequest;
