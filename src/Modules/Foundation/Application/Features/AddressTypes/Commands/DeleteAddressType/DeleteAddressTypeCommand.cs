using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.DeleteAddressType;

public sealed record DeleteAddressTypeCommand(
    Guid Id) : IRequest;
