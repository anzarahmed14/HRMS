using HRMS.Modules.Companies.Application.Features.Companies.DTOs;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : IRequest<IReadOnlyList<CompanyDto>>
{
}
