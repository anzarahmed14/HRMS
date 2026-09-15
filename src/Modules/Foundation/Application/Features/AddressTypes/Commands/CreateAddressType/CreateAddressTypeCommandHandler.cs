using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.CreateAddressType;

public sealed class CreateAddressTypeCommandHandler
    : IRequestHandler<CreateAddressTypeCommand, Guid>
{
    private readonly IWriteRepository<AddressType, Guid> _repository;
    private readonly AddressTypeBusinessRules _businessRules;

    public CreateAddressTypeCommandHandler(
        IWriteRepository<AddressType, Guid> repository,
        AddressTypeBusinessRules businessRules)
    {
        _repository = repository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateAddressTypeCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var addressType = new AddressType
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsActive = request.IsActive
        };

        await _repository.AddAsync(
            addressType,
            cancellationToken);

        return addressType.Id;
    }
}
