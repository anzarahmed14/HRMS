using HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using HRMS.Modules.Leave.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveTypeCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeaveType/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeaveTypeCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route id and command id must match.");

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
            new DeleteLeaveTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveTypeByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
}


