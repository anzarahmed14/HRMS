using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.CreateIdentifierType;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.DeleteIdentifierType;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Commands.UpdateIdentifierType;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypeById;
using HRMS.Modules.Foundation.Application.Features.IdentifierTypes.Queries.GetIdentifierTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentifierTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentifierTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateIdentifierTypeCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/IdentifierTypes/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetIdentifierTypeByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetIdentifierTypesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateIdentifierTypeCommand command,
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
            new DeleteIdentifierTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
}
