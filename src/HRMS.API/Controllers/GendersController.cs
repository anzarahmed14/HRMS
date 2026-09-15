using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.Genders.Commands.CreateGender;
using HRMS.Modules.Foundation.Application.Features.Genders.Commands.DeleteGender;
using HRMS.Modules.Foundation.Application.Features.Genders.Commands.UpdateGender;
using HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenderById;
using HRMS.Modules.Foundation.Application.Features.Genders.Queries.GetGenders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GendersController : ControllerBase
{
    private readonly IMediator _mediator;

    public GendersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateGenderCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/Genders/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetGenderByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetGendersQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateGenderCommand command,
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
            new DeleteGenderCommand(id),
            cancellationToken);

        return NoContent();
    }
}
