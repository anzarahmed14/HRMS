using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.CreateLeaveDayPart;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.DeleteLeaveDayPart;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.Commands.UpdateLeaveDayPart;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayPartById;
using HRMS.Modules.Leave.Application.Features.LeaveDayParts.Queries.GetLeaveDayParts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveDayPartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveDayPartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeaveDayPartCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeaveDayParts/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveDayPartByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeaveDayPartsQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeaveDayPartCommand command,
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
            new DeleteLeaveDayPartCommand(id),
            cancellationToken);

        return NoContent();
    }
}
