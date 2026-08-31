using HRMS.Application.Features.Certifications.DTOs;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Employee.Domain.Entities;
using MediatR;

namespace HRMS.Application.Features.Certifications.Queries.GetEmployeeCertifications;

public sealed class GetEmployeeCertificationsQueryHandler
    : IRequestHandler<
        GetEmployeeCertificationsQuery,
        PagedResult<EmployeeCertificationDto>>
{
    private readonly IReadRepository<EmployeeCertification, Guid> _repository;

    public GetEmployeeCertificationsQueryHandler(
        IReadRepository<EmployeeCertification, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeCertificationDto>> Handle(
        GetEmployeeCertificationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPagedAsync(
            request.Request,
            predicate: x => x.EmployeeId == request.EmployeeId,
            cancellationToken: cancellationToken);

        return new PagedResult<EmployeeCertificationDto>
        {
            Items = result.Items
                .Where(x => !x.IsDeleted)
                .Select(x => new EmployeeCertificationDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    CertificationId = x.CertificationId,
                    CertificationNumber = x.CertificationNumber,
                    IssueDate = x.IssueDate,
                    ExpiryDate = x.ExpiryDate,
                    CredentialUrl = x.CredentialUrl,
                    IsVerified = x.IsVerified,
                    VerifiedOn = x.VerifiedOn,
                    IsActive = x.IsActive
                })
                .ToList(),

            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
