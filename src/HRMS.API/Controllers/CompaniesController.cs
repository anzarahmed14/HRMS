using HRMS.Modules.Companies.Application.Features.Companies.Commands.CreateCompany;
using HRMS.Modules.Companies.Application.Features.Companies.Commands.DeleteCompany;
using HRMS.Modules.Companies.Application.Features.Companies.Commands.UpdateCompany;
using HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanies;
using HRMS.Modules.Companies.Application.Features.Companies.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"api/Companies/{id}",
            id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCompaniesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCompanyByIdQuery
            {
                Id = id
            },
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteCompanyCommand
            {
                Id = id
            },
            cancellationToken);

        return NoContent();
    }
}
