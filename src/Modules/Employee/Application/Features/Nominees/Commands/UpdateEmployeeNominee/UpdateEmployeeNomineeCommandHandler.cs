using AutoMapper;
using HRMS.Application.Features.Nominees.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Nominees.Commands.UpdateEmployeeNominee;

public class UpdateEmployeeNomineeCommandHandler
    : IRequestHandler<UpdateEmployeeNomineeCommand>
{
    private readonly IReadRepository<EmployeeNominee, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeNominee, Guid> _writeRepository;
    private readonly EmployeeNomineeBusinessRules _businessRules;
    private readonly IMapper _mapper;

    public UpdateEmployeeNomineeCommandHandler(
        IReadRepository<EmployeeNominee, Guid> readRepository,
        IWriteRepository<EmployeeNominee, Guid> writeRepository,
        EmployeeNomineeBusinessRules businessRules,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
        _mapper = mapper;
    }

    public async Task Handle(
        UpdateEmployeeNomineeCommand request,
        CancellationToken cancellationToken)
    {
        var nominee = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (nominee is null)
        {
            throw new InvalidOperationException(
                "Employee nominee could not be loaded.");
        }

        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureRelationshipExistsAsync(
            request.RelationshipId,
            cancellationToken);

        await _businessRules.EnsureNomineeNotDuplicateAsync(
            request.EmployeeId,
            request.Name,
            request.RelationshipId,
            request.Id,
            cancellationToken);

        _mapper.Map(request, nominee);

        await _writeRepository.UpdateAsync(
            nominee,
            cancellationToken);
    }
}
