using HRMS.Application.Features.EmployeeContacts.Commands.CreateEmployeeContact;
using HRMS.Application.Features.EmployeeContacts.Commands.DeleteEmployeeContact;
using HRMS.Application.Features.EmployeeContacts.Commands.UpdateEmployeeContact;
using HRMS.Application.Features.EmployeeContacts.DTOs;
using HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContactById;
using HRMS.Application.Features.EmployeeContacts.Queries.GetEmployeeContacts;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class EmployeeContactsController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeContactsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeContactCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(EmployeeContactDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeContactByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(PagedResult<EmployeeContactDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeContactsQuery(
                employeeId,
                request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeContactCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteEmployeeContactCommand(id),
            cancellationToken);

        return NoContent();
    }
}
