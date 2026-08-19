using AutoMapper;
using HRMS.BuildingBlocks.Application.Abstractions.Persistence;
using HRMS.Modules.Companies.Application.Features.Companies.BusinessRules;
using HRMS.Modules.Companies.Domain.Entities;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler
    : IRequestHandler<CreateCompanyCommand, Guid>
{
    private readonly IWriteRepository<Company, Guid> _companyWriteRepository;
    private readonly CompanyBusinessRules _companyRules;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(
        IWriteRepository<Company, Guid> companyWriteRepository,
        CompanyBusinessRules companyRules,
        IMapper mapper)
    {
        _companyWriteRepository = companyWriteRepository;
        _companyRules = companyRules;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        await _companyRules.EnsureCompanyCodeUniqueAsync(
            request.CompanyCode,
            cancellationToken);

        var company = _mapper.Map<Company>(request);

        await _companyWriteRepository.AddAsync(
            company,
            cancellationToken);

        return company.Id;
    }
}