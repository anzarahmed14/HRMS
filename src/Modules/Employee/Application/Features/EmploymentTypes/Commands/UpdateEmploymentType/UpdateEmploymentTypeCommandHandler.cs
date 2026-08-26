using AutoMapper;
using HRMS.Application.Features.EmploymentTypes.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Commands.UpdateEmploymentType;

public class UpdateEmploymentTypeCommandHandler
    : IRequestHandler<UpdateEmploymentTypeCommand>
{
    private readonly IReadRepository<EmploymentType, Guid> _readRepository;
    private readonly IWriteRepository<EmploymentType, Guid> _writeRepository;
    private readonly EmploymentTypeBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmploymentTypeCommandHandler(
        IReadRepository<EmploymentType, Guid> readRepository,
        IWriteRepository<EmploymentType, Guid> writeRepository,
        EmploymentTypeBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmploymentTypeCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Employment type must exist
        await _businessRules.EnsureEmploymentTypeExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Code must be unique
        await _businessRules.EnsureCodeUniqueAsync(
            request.Code,
            request.Id,
            cancellationToken);

        // 3. Name must be unique
        await _businessRules.EnsureNameUniqueAsync(
            request.Name,
            request.Id,
            cancellationToken);

        // 4. Get employment type
        var employmentType = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        // This should never be null because of EnsureEmploymentTypeExistsAsync().
        if (employmentType is null)
        {
            throw new InvalidOperationException(
                "Employment type could not be loaded.");
        }

        // 5. Map request ? existing entity
        _mapper.Map(request, employmentType);

        // 6. Update database
        await _writeRepository.UpdateAsync(
            employmentType,
            cancellationToken);
    }
}
