using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.CreateLeaveRequestStatus;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.DeleteLeaveRequestStatus;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Commands.UpdateLeaveRequestStatus;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatusById;
using HRMS.Modules.Leave.Application.Features.LeaveRequestStatuses.Queries.GetLeaveRequestStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveRequestStatusCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeaveRequestStatuses/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveRequestStatusByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveRequestStatusesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeaveRequestStatusCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route id and command id must match.");
        }

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
            new DeleteLeaveRequestStatusCommand(id),
            cancellationToken);

        return NoContent();
    }
}
