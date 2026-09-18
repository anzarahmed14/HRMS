using HRMS.Modules.Identity.Application.Authorization;
using HRMS.Modules.Identity.Application.Features.PermissionCatalog.Commands.SynchronizePermissionCatalog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Identity;

/// <summary>
/// Administrative endpoint for reconciling the code-side permission
/// catalog with the database. Deliberately not invoked automatically at
/// startup — an administrator triggers it explicitly, and it is
/// idempotent, so triggering it more than once is always safe.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PermissionCatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionCatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("synchronize")]
    [ProducesResponseType(
        typeof(PermissionSynchronizationResult),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> Synchronize(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SynchronizePermissionCatalogCommand(),
            cancellationToken);

        return Ok(result);
    }
}