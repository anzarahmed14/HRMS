using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.DTOs;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;
using HRMS.Modules.Leave.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Leave;

[ApiController]
[Route("api/company-holidays")]
public class CompanyHolidaysController : ControllerBase
{
    private readonly ISender _sender;

    public CompanyHolidaysController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyHolidayCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CompanyHolidayDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetCompanyHolidayByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<CompanyHolidayDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetCompanyHolidaysQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCompanyHolidayCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteCompanyHolidayCommand(id),
            cancellationToken);

        return NoContent();
    }
}
