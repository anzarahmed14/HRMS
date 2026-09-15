using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypeById;

public sealed class GetAddressTypeByIdQueryHandler
    : IRequestHandler<GetAddressTypeByIdQuery, AddressTypeDto>
{
    private readonly IReadRepository<AddressType, Guid> _repository;

    public GetAddressTypeByIdQueryHandler(
        IReadRepository<AddressType, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<AddressTypeDto> Handle(
        GetAddressTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var addressType = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (addressType is null || addressType.IsDeleted)
        {
            throw new NotFoundException(
                "AddressType",
                request.Id);
        }

        return new AddressTypeDto
        {
            Id = addressType.Id,
            Code = addressType.Code,
            Name = addressType.Name,
            Description = addressType.Description,
            IsActive = addressType.IsActive
        };
    }
}
