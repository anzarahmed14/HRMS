using HRMS.Modules.Foundation.Application.Features.Relationships.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Queries.GetRelationshipById;

public record GetRelationshipByIdQuery(
    Guid Id) : IRequest<RelationshipDto>;
