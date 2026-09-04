using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;
using HRMS.Modules.Companies.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IReadRepository<Company, Guid> _companyReadRepository;
    private readonly IWriteRepository<Company, Guid> _companyWriteRepository;
    private readonly CompanyBusinessRules _companyRules;

    public DeleteCompanyCommandHandler(
        IReadRepository<Company, Guid> companyReadRepository,
        IWriteRepository<Company, Guid> companyWriteRepository,
        CompanyBusinessRules companyRules)
    {
        _companyReadRepository = companyReadRepository;
        _companyWriteRepository = companyWriteRepository;
        _companyRules = companyRules;
    }

    public async Task Handle(
        DeleteCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyReadRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        await _companyRules.EnsureCompanyExistsAsync(
            request.Id,
            cancellationToken);

        await _companyWriteRepository.DeleteAsync(
            company!,
            cancellationToken);
    }
}
