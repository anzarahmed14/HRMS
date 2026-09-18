using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateUser;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.Login;
using HRMS.Modules.Identity.Application.Features.Identity.DTOs;
using HRMS.Modules.Identity.Application.Features.Identity.Queries.GetEffectivePermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    public AuthController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var effectivePermissions = _userContext.UserId.HasValue
            ? await _mediator.Send(
                new GetEffectivePermissionsQuery(_userContext.UserId.Value),
                cancellationToken)
            : new EffectivePermissionsDto();

        return Ok(new
        {
            UserId = _userContext.UserId,
            EmployeeId = _userContext.EmployeeId,
            UserName = _userContext.UserName,
            IsAuthenticated = _userContext.IsAuthenticated,
            Roles = effectivePermissions.Roles,
            Permissions = effectivePermissions.Permissions
        });
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(new
        {
            Id = userId,
            Message = "User created successfully."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }
}