using HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.CreateLeaveYear;
using HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.UpdateLeaveYear;
using HRMS.Modules.Leave.Application.Features.LeaveYears.Commands.DeleteLeaveYear;
using HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYearById;
using HRMS.Modules.Leave.Application.Features.LeaveYears.Queries.GetLeaveYears;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.BuildingBlocks.Application.Pagination;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveYearController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveYearController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveYearCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeaveYear/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeaveYearCommand command,
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
            new DeleteLeaveYearCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveYearByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveYearsQuery(request),
            cancellationToken);

        return Ok(result);
    }
}



