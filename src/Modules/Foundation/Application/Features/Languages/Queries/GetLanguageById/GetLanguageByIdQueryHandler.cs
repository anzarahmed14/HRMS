using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Foundation.Application.Features.Languages.DTOs;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Languages.Queries.GetLanguageById;

public class GetLanguageByIdQueryHandler
    : IRequestHandler<GetLanguageByIdQuery, LanguageDto>
{
    private readonly IReadRepository<Language, Guid> _repository;

    public GetLanguageByIdQueryHandler(
        IReadRepository<Language, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<LanguageDto> Handle(
        GetLanguageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null || entity.IsDeleted)
        {
            throw new NotFoundException(
                "Language",
                request.Id);
        }

        return new LanguageDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsActive = entity.IsActive
        };
    }
}
