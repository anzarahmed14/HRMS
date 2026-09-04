using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;
using HRMS.Modules.Companies.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;
    private readonly IWriteRepository<Company, Guid> _companyWriteRepository;
    private readonly CompanyBusinessRules _companyRules;

    public UpdateCompanyCommandHandler(
        IReadRepository<Company, Guid> companyReadRepository,
        IWriteRepository<Company, Guid> companyWriteRepository,
        CompanyBusinessRules companyRules)
    {
        _companyReadRepository = companyReadRepository;
        _companyWriteRepository = companyWriteRepository;
        _companyRules = companyRules;
    }

    public async Task Handle(
        UpdateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        await _companyRules.EnsureCompanyExistsAsync(
            request.Id,
            cancellationToken);

        await _companyRules.EnsureCompanyCodeUniqueAsync(
            request.CompanyCode,
            request.Id,
            cancellationToken);

        company!.CompanyCode = request.CompanyCode;
        company.Name = request.Name;
        company.IsActive = request.IsActive;

        await _companyWriteRepository.UpdateAsync(
            company,
            cancellationToken);
    }
}
