using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.BuildingBlocks.Application.Exceptions;
using HRMS.Modules.Companies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;


public class CompanyBusinessRules
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;

    public CompanyBusinessRules(
        IReadRepository<Company, Guid> companyReadRepository)
    {
        _companyReadRepository = companyReadRepository;
    }

    public async Task EnsureCompanyExistsAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _companyReadRepository.GetByIdAsync(
            companyId,
            cancellationToken);

        if (company is null)
        {
            throw new NotFoundException(
                "Company",
                companyId);
        }
    }

    public async Task EnsureCompanyCodeUniqueAsync(
        string companyCode,
        CancellationToken cancellationToken = default)
    {
        var exists = await _companyReadRepository.AnyAsync(
            x => x.CompanyCode == companyCode,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Company code already exists.");
        }
    }

    public async Task EnsureCompanyCodeUniqueAsync(
        string companyCode,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _companyReadRepository.AnyAsync(
            x => x.CompanyCode == companyCode &&
                 x.Id != companyId,
            cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "Company code already exists.");
        }
    }
}

