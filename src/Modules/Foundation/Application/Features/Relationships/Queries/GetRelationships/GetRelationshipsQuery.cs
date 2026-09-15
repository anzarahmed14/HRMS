using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Relationships.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Relationships.Queries.GetRelationships;

public record GetRelationshipsQuery(
    PagedRequest Request) : IRequest<PagedResult<RelationshipDto>>;
