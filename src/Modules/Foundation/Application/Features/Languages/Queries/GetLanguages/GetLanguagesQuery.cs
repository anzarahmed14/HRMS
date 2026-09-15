using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Languages.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Queries.GetLanguages;

public record GetLanguagesQuery(
    PagedRequest Request) : IRequest<PagedResult<LanguageDto>>;
