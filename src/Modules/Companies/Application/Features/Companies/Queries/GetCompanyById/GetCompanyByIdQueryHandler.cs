using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;
using HRMS.Modules.Companies.Application.Features.Companies.DTOs;
using HRMS.Modules.Companies.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;
    private readonly CompanyBusinessRules _companyRules;

    public GetCompanyByIdQueryHandler(
        IReadRepository<Company, Guid> companyReadRepository,
        CompanyBusinessRules companyRules)
    {
        _companyReadRepository = companyReadRepository;
        _companyRules = companyRules;
    }

    public async Task<CompanyDto> Handle(
        GetCompanyByIdQuery request,
        CancellationToken cancellationToken)
    {
        await _companyRules.EnsureCompanyExistsAsync(
            request.Id,
            cancellationToken);

        var company = await _companyReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        return new CompanyDto
        {
            Id = company!.Id,
            CompanyCode = company.CompanyCode,
            Name = company.Name,
            IsActive = company.IsActive
        };
    }
}
