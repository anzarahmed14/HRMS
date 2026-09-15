using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.UpdateAddressType;

public sealed record UpdateAddressTypeCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive) : IRequest;
