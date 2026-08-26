using HRMS.Application.Features.EmergencyContacts.Commands.CreateEmergencyContact;
using HRMS.Application.Features.EmergencyContacts.Commands.DeleteEmergencyContact;
using HRMS.Application.Features.EmergencyContacts.Commands.UpdateEmergencyContact;
using HRMS.Application.Features.EmergencyContacts.DTOs;
using HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContactById;
using HRMS.Application.Features.EmergencyContacts.Queries.GetEmergencyContacts;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class EmergencyContactsController : ControllerBase
{
    private readonly ISender _sender;

    public EmergencyContactsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmergencyContactCommand command,
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
        typeof(EmergencyContactDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmergencyContactByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(PagedResult<EmergencyContactDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmergencyContactsQuery(
                employeeId,
                request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmergencyContactCommand command,
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
            new DeleteEmergencyContactCommand(id),
            cancellationToken);

        return NoContent();
    }
}
