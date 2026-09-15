using HRMS.Modules.Foundation.Application.Features.AddressTypes.DTOs;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypeById;

public sealed record GetAddressTypeByIdQuery(
    Guid Id) : IRequest<AddressTypeDto>;
