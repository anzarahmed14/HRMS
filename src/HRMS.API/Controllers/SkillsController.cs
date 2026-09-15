using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Skills.Commands.CreateSkill;
using HRMS.Modules.Foundation.Application.Features.Skills.Commands.DeleteSkill;
using HRMS.Modules.Foundation.Application.Features.Skills.Commands.UpdateSkill;
using HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkillById;
using HRMS.Modules.Foundation.Application.Features.Skills.Queries.GetSkills;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSkillCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/Skills/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSkillByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSkillsQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateSkillCommand command,
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
            new DeleteSkillCommand(id),
            cancellationToken);

        return NoContent();
    }
}
