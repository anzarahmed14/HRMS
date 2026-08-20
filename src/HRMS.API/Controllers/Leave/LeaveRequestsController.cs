using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.ApproveLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CancelLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.DeleteLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.RejectLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.SubmitLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Commands.UpdateLeaveRequest;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.DTOs;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using HRMS.Modules.Leave.Application.Features.LeaveRequests.Queries.GetLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Leave;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public LeaveRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveRequestCommand command,
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
    [ProducesResponseType(typeof(LeaveRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetLeaveRequestByIdQuery(id);

        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    [ProducesResponseType(
    typeof(PagedResult<LeaveRequestDto>),
    StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
    [FromQuery] GetLeaveRequestsQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteLeaveRequestCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateLeaveRequestCommand command,
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
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Submit(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new SubmitLeaveRequestCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(
    Guid id,
    [FromBody] ApproveLeaveRequestCommand command,
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
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reject(
    Guid id,
    [FromBody] RejectLeaveRequestCommand command,
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
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
    Guid id,
    [FromBody] CancelLeaveRequestCommand command,
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
}
