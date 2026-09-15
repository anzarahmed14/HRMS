using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.UpdateAddressType;

public sealed class UpdateAddressTypeCommandHandler
    : IRequestHandler<UpdateAddressTypeCommand>
{
    private readonly IReadRepository<AddressType, Guid> _readRepository;
    private readonly IWriteRepository<AddressType, Guid> _writeRepository;
    private readonly AddressTypeBusinessRules _businessRules;

    public UpdateAddressTypeCommandHandler(
        IReadRepository<AddressType, Guid> readRepository,
        IWriteRepository<AddressType, Guid> writeRepository,
        AddressTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        UpdateAddressTypeCommand request,
        CancellationToken cancellationToken)
    {
        var addressType = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (addressType is null || addressType.IsDeleted)
        {
            await _businessRules.EnsureAddressTypeExistsAsync(
                request.Id,
                cancellationToken);

            return;
        }

        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            request.Id,
            cancellationToken);

        addressType.Code = code;
        addressType.Name = request.Name.Trim();
        addressType.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();
        addressType.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            addressType,
            cancellationToken);
    }
}
