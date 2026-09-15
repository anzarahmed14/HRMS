using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.CreateMaritalStatus;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.DeleteMaritalStatus;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Commands.UpdateMaritalStatus;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatusById;
using HRMS.Modules.Foundation.Application.Features.MaritalStatuses.Queries.GetMaritalStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaritalStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaritalStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMaritalStatusCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/MaritalStatuses/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMaritalStatusByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMaritalStatusesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMaritalStatusCommand command,
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
            new DeleteMaritalStatusCommand(id),
            cancellationToken);

        return NoContent();
    }
}
