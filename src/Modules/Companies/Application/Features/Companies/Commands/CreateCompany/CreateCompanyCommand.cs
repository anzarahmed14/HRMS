using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.CreateCompany;

public record CreateCompanyCommand : IRequest<Guid>
{
    public string CompanyCode { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}