using AutoMapper;
using HRMS.Application.Features.Educations.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Educations.Commands.UpdateEmployeeEducation;

public class UpdateEmployeeEducationCommandHandler
    : IRequestHandler<UpdateEmployeeEducationCommand>
{
    private readonly IReadRepository<EmployeeEducation, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeEducation, Guid> _writeRepository;
    private readonly EmployeeEducationBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeEducationCommandHandler(
        IReadRepository<EmployeeEducation, Guid> readRepository,
        IWriteRepository<EmployeeEducation, Guid> writeRepository,
        EmployeeEducationBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeEducationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException(
                "Employee education could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        if (request.IsHighestQualification)
        {
            await _businessRules.EnsureHighestQualificationAvailableAsync(
                request.EmployeeId,
                request.Id,
                cancellationToken);
        }

        _mapper.Map(request, entity);

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
