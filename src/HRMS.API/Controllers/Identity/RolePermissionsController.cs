using HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignRolePermission;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveRolePermission;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetRolePermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class RolePermissionsController : ControllerBase
{
    private readonly ISender _sender;

    public RolePermissionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Assign(
        [FromBody] AssignRolePermissionCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("role/{roleId:guid}")]
    public async Task<IActionResult> GetByRole(
        Guid roleId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRolePermissionsQuery(roleId),
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{roleId:guid}/{permissionId:guid}")]
    public async Task<IActionResult> Remove(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new RemoveRolePermissionCommand
            {
                RoleId = roleId,
                PermissionId = permissionId
            },
            cancellationToken);

        return NoContent();
    }
}
