using HRMS.Application.Features.GovernmentIdentifiers.Commands.CreateGovernmentIdentifier;
using HRMS.Application.Features.GovernmentIdentifiers.Commands.DeleteGovernmentIdentifier;
using HRMS.Application.Features.GovernmentIdentifiers.Commands.UpdateGovernmentIdentifier;
using HRMS.Application.Features.GovernmentIdentifiers.DTOs;
using HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifierById;
using HRMS.Application.Features.GovernmentIdentifiers.Queries.GetGovernmentIdentifiers;
using HRMS.Application.Features.GovernmentIdentifiers.Commands.VerifyGovernmentIdentifier;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class GovernmentIdentifiersController : ControllerBase
{
    private readonly ISender _sender;

    public GovernmentIdentifiersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateGovernmentIdentifierCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(GovernmentIdentifierDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetGovernmentIdentifierByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(PagedResult<GovernmentIdentifierDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetGovernmentIdentifiersQuery(
                employeeId,
                request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateGovernmentIdentifierCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }


    [HttpPost("{id:guid}/verify")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Verify(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new VerifyGovernmentIdentifierCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteGovernmentIdentifierCommand(id),
            cancellationToken);

        return NoContent();
    }
}

