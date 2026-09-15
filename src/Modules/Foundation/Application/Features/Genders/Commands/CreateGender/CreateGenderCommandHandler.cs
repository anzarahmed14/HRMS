using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Foundation.Application.Features.Genders.BusinessRules;
using HRMS.Modules.Foundation.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Foundation.Application.Features.Genders.Commands.CreateGender;

public class CreateGenderCommandHandler
    : IRequestHandler<CreateGenderCommand, Guid>
{
    private readonly IWriteRepository<Gender, Guid> _writeRepository;
    private readonly GenderBusinessRules _businessRules;

    public CreateGenderCommandHandler(
        IWriteRepository<Gender, Guid> writeRepository,
        GenderBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateGenderCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();

        await _businessRules.EnsureCodeUniqueAsync(
            code,
            cancellationToken);

        var entity = new Gender
        {
            Code = code,
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            IsActive = request.IsActive
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
