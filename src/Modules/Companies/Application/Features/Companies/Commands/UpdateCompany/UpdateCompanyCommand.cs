using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest
{
    public Guid Id { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
