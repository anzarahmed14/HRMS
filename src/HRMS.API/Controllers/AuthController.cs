using HRMS.BuildingBlocks.Application.Abstractions;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.CreateUser;
using HRMS.Modules.Identity.Application.Features.Identity.Commands.Login;
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
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = _userContext.UserId,
            EmployeeId = _userContext.EmployeeId,
            UserName = _userContext.UserName,
            IsAuthenticated = _userContext.IsAuthenticated
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