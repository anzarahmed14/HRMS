using HRMS.Application.Features.EmploymentTypes.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.EmploymentTypes.Commands.DeleteEmploymentType;

public class DeleteEmploymentTypeCommandHandler
    : IRequestHandler<DeleteEmploymentTypeCommand>
{
    private readonly IReadRepository<EmploymentType, Guid> _readRepository;
    private readonly IWriteRepository<EmploymentType, Guid> _writeRepository;
    private readonly EmploymentTypeBusinessRules _businessRules;

    public DeleteEmploymentTypeCommandHandler(
        IReadRepository<EmploymentType, Guid> readRepository,
        IWriteRepository<EmploymentType, Guid> writeRepository,
        EmploymentTypeBusinessRules businessRules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task Handle(
        DeleteEmploymentTypeCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Make sure employment type exists
        await _businessRules.EnsureEmploymentTypeExistsAsync(
            request.Id,
            cancellationToken);

        // 2. Get employment type
        var employmentType = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employmentType is null)
        {
            throw new InvalidOperationException(
                "Employment type could not be loaded.");
        }

        // 3. Delete employment type
        await _writeRepository.DeleteAsync(
            employmentType,
            cancellationToken);
    }
}
