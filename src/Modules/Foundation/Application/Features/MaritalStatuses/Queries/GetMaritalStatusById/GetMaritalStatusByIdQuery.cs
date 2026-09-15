using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatusById;

public record GetMaritalStatusByIdQuery(
    Guid Id) : IRequest<MaritalStatusDto>;
