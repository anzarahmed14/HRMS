using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Commands.DeleteRelationship;

public record DeleteRelationshipCommand(
    Guid Id) : IRequest;
