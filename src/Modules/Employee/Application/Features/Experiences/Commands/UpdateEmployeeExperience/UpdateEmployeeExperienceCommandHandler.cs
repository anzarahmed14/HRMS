using AutoMapper;
using HRMS.Application.Features.Experiences.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Experiences.Commands.UpdateEmployeeExperience;

public class UpdateEmployeeExperienceCommandHandler
    : IRequestHandler<UpdateEmployeeExperienceCommand>
{
    private readonly IReadRepository<EmployeeExperience, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeExperience, Guid> _writeRepository;
    private readonly EmployeeExperienceBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeExperienceCommandHandler(
        IReadRepository<EmployeeExperience, Guid> readRepository,
        IWriteRepository<EmployeeExperience, Guid> writeRepository,
        EmployeeExperienceBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeExperienceCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException(
                "Employee experience could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        _mapper.Map(request, entity);

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
