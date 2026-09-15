using HRMS.Modules.Leave.Application.Features.LeaveDayParts.DTOs;
using MediatR;

namespace HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayPartById;

public record GetLeaveDayPartByIdQuery(Guid Id)
    : IRequest<LeaveDayPartDto>;
