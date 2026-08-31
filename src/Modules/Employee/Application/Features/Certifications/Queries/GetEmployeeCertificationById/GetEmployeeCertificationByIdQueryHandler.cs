using HRMS.Application.Features.Certifications.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Certifications.Queries.GetEmployeeCertificationById;

public class GetEmployeeCertificationByIdQueryHandler
    : IRequestHandler<GetEmployeeCertificationByIdQuery, EmployeeCertificationDto?>
{
    private readonly IReadRepository<EmployeeCertification, Guid> _repository;

    public GetEmployeeCertificationByIdQueryHandler(
        IReadRepository<EmployeeCertification, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeCertificationDto?> Handle(
        GetEmployeeCertificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity is null)
            return null;

        return new EmployeeCertificationDto
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            CertificationId = entity.CertificationId,
            CertificationNumber = entity.CertificationNumber,
            IssueDate = entity.IssueDate,
            ExpiryDate = entity.ExpiryDate,
            CredentialUrl = entity.CredentialUrl,
            IsVerified = entity.IsVerified,
            VerifiedOn = entity.VerifiedOn,
            IsActive = entity.IsActive
        };
    }
}
