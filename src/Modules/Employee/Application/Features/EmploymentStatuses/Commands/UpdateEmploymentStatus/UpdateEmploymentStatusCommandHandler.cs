using AutoMapper;
using HRMS.Application.Features.EmploymentStatuses.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentStatuses.Commands.UpdateEmploymentStatus;

public class UpdateEmploymentStatusCommandHandler
    : IRequestHandler<UpdateEmploymentStatusCommand>
{
    private readonly IReadRepository<EmploymentStatus, Guid> _readRepository;
    private readonly IWriteRepository<EmploymentStatus, Guid> _writeRepository;
    private readonly EmploymentStatusBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmploymentStatusCommandHandler(
        IReadRepository<EmploymentStatus, Guid> readRepository,
        IWriteRepository<EmploymentStatus, Guid> writeRepository,
        EmploymentStatusBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmploymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmploymentStatusExistsAsync(
            request.Id,
            cancellationToken);

        await _businessRules.EnsureCodeUniqueAsync(
            request.Code,
            request.Id,
            cancellationToken);

        await _businessRules.EnsureNameUniqueAsync(
            request.Name,
            request.Id,
            cancellationToken);

        var employmentStatus = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employmentStatus is null)
        {
            throw new InvalidOperationException(
                "Employment status could not be loaded.");
        }

        _mapper.Map(request, employmentStatus);

        await _writeRepository.UpdateAsync(
            employmentStatus,
            cancellationToken);
    }
}
