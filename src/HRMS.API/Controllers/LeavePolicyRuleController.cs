using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.CreateLeavePolicyRule;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.UpdateLeavePolicyRule;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Commands.DeleteLeavePolicyRule;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRuleById;
using HRMS.Modules.Leave.Application.Features.LeavePolicyRules.Queries.GetLeavePolicyRules;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.BuildingBlocks.Application.Pagination;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeavePolicyRuleController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeavePolicyRuleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeavePolicyRuleCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/LeavePolicyRule/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateLeavePolicyRuleCommand command,
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
            new DeleteLeavePolicyRuleCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeavePolicyRuleByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetLeavePolicyRulesQuery(request),
            cancellationToken);

        return Ok(result);
    }
}



