using HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateRole;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateRole;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateRole;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoleById;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly ISender _sender;

    public RoleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRolesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRoleByIdQuery(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRoleCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route id and command id must match.");
        }

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeactivateRoleCommand(id),
            cancellationToken);

        return NoContent();
    }
}
