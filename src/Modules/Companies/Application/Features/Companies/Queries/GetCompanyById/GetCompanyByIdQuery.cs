using HRMS.Modules.Companies.Application.Features.Companies.DTOs;
using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQuery : IRequest<CompanyDto>
{
    public Guid Id { get; set; }
}
