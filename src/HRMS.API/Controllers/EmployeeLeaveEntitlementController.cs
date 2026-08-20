using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.CreateEmployeeLeaveEntitlement;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.UpdateEmployeeLeaveEntitlement;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Commands.DeleteEmployeeLeaveEntitlement;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlementById;
using HRMS.Modules.Leave.Application.Features.EmployeeLeaveEntitlements.Queries.GetEmployeeLeaveEntitlements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.BuildingBlocks.Application.Pagination;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeLeaveEntitlementController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeLeaveEntitlementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeLeaveEntitlementCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/EmployeeLeaveEntitlement/{id}",
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeLeaveEntitlementCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route id and command id must match.");

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
            new DeleteEmployeeLeaveEntitlementCommand(id),
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEmployeeLeaveEntitlementByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEmployeeLeaveEntitlementsQuery(request),
            cancellationToken);

        return Ok(result);
    }
}



