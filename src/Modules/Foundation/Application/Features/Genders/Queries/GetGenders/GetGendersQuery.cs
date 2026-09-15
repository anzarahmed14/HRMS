using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Genders.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenders;

public record GetGendersQuery(
    PagedRequest Request) : IRequest<PagedResult<GenderDto>>;
