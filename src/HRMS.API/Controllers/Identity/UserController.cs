using HRMS.Modules.Identity.Application.Features.Identity.Commands.ChangePassword;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateUser;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.DeactivateUser;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.ResetPassword;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.UpdateUser;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUserById;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetUsersQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetUserByIdQuery(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command,
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
        [FromBody] UpdateUserCommand command,
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
            new DeactivateUserCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(
    Guid id,
    [FromBody] ChangePasswordCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.UserId)
        {
            return BadRequest(
                "Route id and command user id must match.");
        }

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:guid}/reset-password")]
    [Authorize(Policy = "User.ResetPassword")]
    public async Task<IActionResult> ResetPassword(
    Guid id,
    [FromBody] ResetPasswordCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.UserId)
        {
            return BadRequest(
                "Route id and command user id must match.");
        }

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
}
