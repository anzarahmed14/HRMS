using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Companies.Application.Features.Companies.DTOs;
using HRMS.Modules.Companies.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanies;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, IReadOnlyList<CompanyDto>>
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;

    public GetCompaniesQueryHandler(
        IReadRepository<Company, Guid> companyReadRepository)
    {
        _companyReadRepository = companyReadRepository;
    }

    public async Task<IReadOnlyList<CompanyDto>> Handle(
        GetCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await _companyReadRepository.GetAllAsync(
            cancellationToken);

        return companies
            .Select(x => new CompanyDto
            {
                Id = x.Id,
                CompanyCode = x.CompanyCode,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .ToList();
    }
}
