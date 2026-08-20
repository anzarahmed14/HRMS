using HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.CreateLeavePolicy;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.UpdateLeavePolicy;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.Commands.DeleteLeavePolicy;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicyById;
using HRMS.Modules.Leave.Application.Features.LeavePolicies.Queries.GetLeavePolicies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.BuildingBlocks.Application.Pagination;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeavePolicyController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeavePolicyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeavePolicyCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeavePolicy/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeavePolicyCommand command,
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
            new DeleteLeavePolicyCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeavePolicyByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeavePoliciesQuery(request),
            cancellationToken);

        return Ok(result);
    }
}



