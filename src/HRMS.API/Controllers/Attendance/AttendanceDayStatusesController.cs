using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.CreateAttendanceDayStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.DeleteAttendanceDayStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Commands.UpdateAttendanceDayStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatusById;
using HRMS.Modules.Attendance.Application.Features.AttendanceDayStatuses.Queries.GetAttendanceDayStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/[controller]")]
public class AttendanceDayStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceDayStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceDayStatusCommand command,
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
        typeof(AttendanceDayStatusDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendanceDayStatusByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AttendanceDayStatusDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendanceDayStatusesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAttendanceDayStatusCommand command,
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
            new DeleteAttendanceDayStatusCommand(id),
            cancellationToken);

        return NoContent();
    }
}
