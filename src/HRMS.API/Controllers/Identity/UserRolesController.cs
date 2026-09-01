using HRMS.Modules.Identity.Application.Features.Identity.Commands.AssignUserRole;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.RemoveUserRole;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly ISender _sender;

    public UserRolesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Assign(
        [FromBody] AssignUserRoleCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetUserRolesQuery(userId),
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{userId:guid}/{roleId:guid}")]
    public async Task<IActionResult> Remove(
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new RemoveUserRoleCommand
            {
                UserId = userId,
                RoleId = roleId
            },
            cancellationToken);

        return NoContent();
    }
}
