using HRMS.Application.Features.GovernmentIdentifiers.DTOs;
using MediatR;

namespace HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifierById;

public record GetGovernmentIdentifierByIdQuery(Guid Id)
    : IRequest<GovernmentIdentifierDto?>;
