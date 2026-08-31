using HRMS.Application.Features.Certifications.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.UpdateEmployeeCertification;

public class UpdateEmployeeCertificationCommandHandler
    : IRequestHandler<UpdateEmployeeCertificationCommand>
{
    private readonly IReadRepository<EmployeeCertification, Guid> _readRepository;
    private readonly IWriteRepository<EmployeeCertification, Guid> _writeRepository;
    private readonly EmployeeCertificationBusinessRules _rules;

    public UpdateEmployeeCertificationCommandHandler(
        IReadRepository<EmployeeCertification, Guid> readRepository,
        IWriteRepository<EmployeeCertification, Guid> writeRepository,
        EmployeeCertificationBusinessRules rules)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _rules = rules;
    }

    public async Task Handle(
        UpdateEmployeeCertificationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _readRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            throw new InvalidOperationException(
                "Employee certification could not be loaded.");

        await _rules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _rules.EnsureCertificationExistsAsync(
            request.CertificationId,
            cancellationToken);

        await _rules.EnsureCertificationAvailableAsync(
            request.EmployeeId,
            request.CertificationId,
            request.Id,
            cancellationToken);

        entity.EmployeeId = request.EmployeeId;
        entity.CertificationId = request.CertificationId;
        entity.CertificationNumber = request.CertificationNumber;
        entity.IssueDate = request.IssueDate;
        entity.ExpiryDate = request.ExpiryDate;
        entity.CredentialUrl = request.CredentialUrl;
        entity.IsActive = request.IsActive;

        await _writeRepository.UpdateAsync(
            entity,
            cancellationToken);
    }
}
