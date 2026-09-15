using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.CreateLeaveYearStatus;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.DeleteLeaveYearStatus;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Commands.UpdateLeaveYearStatus;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatusById;
using HRMS.Modules.Leave.Application.Features.LeaveYearStatuses.Queries.GetLeaveYearStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveYearStatusController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveYearStatusController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveYearStatusCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeaveYearStatus/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeaveYearStatusCommand command,
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
            new DeleteLeaveYearStatusCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveYearStatusByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveYearStatusesQuery(request),
            cancellationToken);

        return Ok(result);
    }
}
