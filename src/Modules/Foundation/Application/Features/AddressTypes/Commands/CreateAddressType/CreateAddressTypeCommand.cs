using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.CreateAddressType;

public sealed record CreateAddressTypeCommand(
    string Code,
    string Name,
    string? Description,
    bool IsActive = true) : IRequest<Guid>;
