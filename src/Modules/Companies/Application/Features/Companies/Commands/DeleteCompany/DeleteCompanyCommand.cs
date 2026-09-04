using MediatR;

namespace HRMS.Modules.Companies.Application.Features.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IRequest
{
    public Guid Id { get; set; }
}
