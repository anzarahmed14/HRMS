using HRMS.Modules.Foundation.Application.Features.Languages.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Queries.GetLanguageById;

public record GetLanguageByIdQuery(
    Guid Id) : IRequest<LanguageDto>;
