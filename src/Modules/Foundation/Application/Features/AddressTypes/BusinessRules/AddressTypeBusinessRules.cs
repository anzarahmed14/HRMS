using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Domain.Entities;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.BusinessRules;

public sealed class AddressTypeBusinessRules
{
    private readonly IReadRepository<AddressType, Guid> _repository;

    public AddressTypeBusinessRules(
        IReadRepository<AddressType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task EnsureAddressTypeExistsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var addressType = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        if (addressType is null || addressType.IsDeleted)
        {
            throw new NotFoundException(
                "AddressType",
                id);
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Address type code already exists.");
        }
    }

    public async Task EnsureCodeUniqueAsync(
        string code,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(
            x => x.Id != id && x.Code == code,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Address type code already exists.");
        }
    }
}