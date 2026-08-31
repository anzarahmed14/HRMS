using HRMS.Application.Features.Certifications.BusinessRules;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Certifications.Commands.CreateEmployeeCertification;

public class CreateEmployeeCertificationCommandHandler
    : IRequestHandler<CreateEmployeeCertificationCommand, Guid>
{
    private readonly IWriteRepository<EmployeeCertification, Guid> _writeRepository;
    private readonly EmployeeCertificationBusinessRules _businessRules;

    public CreateEmployeeCertificationCommandHandler(
        IWriteRepository<EmployeeCertification, Guid> writeRepository,
        EmployeeCertificationBusinessRules businessRules)
    {
        _writeRepository = writeRepository;
        _businessRules = businessRules;
    }

    public async Task<Guid> Handle(
        CreateEmployeeCertificationCommand request,
        CancellationToken cancellationToken)
    {
        await _businessRules.EnsureEmployeeExistsAsync(
            request.EmployeeId,
            cancellationToken);

        await _businessRules.EnsureCertificationExistsAsync(
            request.CertificationId,
            cancellationToken);

        await _businessRules.EnsureCertificationAvailableAsync(
            request.EmployeeId,
            request.CertificationId,
            cancellationToken);

        var entity = new EmployeeCertification
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            CertificationId = request.CertificationId,
            CertificationNumber = request.CertificationNumber,
            IssueDate = request.IssueDate,
            ExpiryDate = request.ExpiryDate,
            CredentialUrl = request.CredentialUrl,
            IsVerified = false,
            VerifiedOn = null,
            IsActive = true
        };

        await _writeRepository.AddAsync(
            entity,
            cancellationToken);

        return entity.Id;
    }
}
