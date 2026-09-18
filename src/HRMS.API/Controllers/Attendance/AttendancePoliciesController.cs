using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.CreateAttendancePolicy;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.UpdateAttendancePolicy;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Commands.DeleteAttendancePolicy;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicyById;
using HRMS.Modules.Attendance.Application.Features.AttendancePolicies.Queries.GetAttendancePolicies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/[controller]")]
public class AttendancePoliciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendancePoliciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendancePolicyCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AttendancePolicyDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendancePolicyByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AttendancePolicyDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendancePoliciesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAttendancePolicyCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteAttendancePolicyCommand(id),
            cancellationToken);

        return NoContent();
    }}

