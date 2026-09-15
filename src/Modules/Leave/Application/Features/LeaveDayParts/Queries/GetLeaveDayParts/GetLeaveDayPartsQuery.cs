using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayParts;

public record GetLeaveDayPartsQuery(
    PagedRequest Request) : IRequest<PagedResult<LeaveDayPartDto>>;
