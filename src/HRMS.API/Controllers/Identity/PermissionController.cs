using HRMS.Modules.Identity.Application.Features.Identity.Commands.CreatePermission;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivatePermission;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdatePermission;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissionById;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly ISender _sender;

    public PermissionController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetPermissionsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetPermissionByIdQuery(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePermissionCommand command,
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
        [FromBody] UpdatePermissionCommand command,
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
            new DeactivatePermissionCommand(id),
            cancellationToken);

        return NoContent();
    }
}
