using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypeById;

public record GetIdentifierTypeByIdQuery(
    Guid Id) : IRequest<IdentifierTypeDto>;
