using HRMS.Modules.Foundation.Application.Features.States.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.States.Queries.GetStateById;

public sealed record GetStateByIdQuery(
    Guid Id) : IRequest<StateDto>;
