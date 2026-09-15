using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Genders.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenderById;

public class GetGenderByIdQueryHandler
    : IRequestHandler<GetGenderByIdQuery, GenderDto>
{
    private readonly IReadRepository<Gender, Guid> _repository;

    public GetGenderByIdQueryHandler(
        IReadRepository<Gender, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<GenderDto> Handle(
        GetGenderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Gender",
                request.Id);
        }

        return new GenderDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}
