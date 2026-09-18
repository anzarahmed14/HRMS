using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.CreateAttendanceRegularizationStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.DeleteAttendanceRegularizationStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Commands.UpdateAttendanceRegularizationStatus;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatusById;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizationStatuses.Queries.GetAttendanceRegularizationStatuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/[controller]")]
public class AttendanceRegularizationStatusesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceRegularizationStatusesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRegularizationStatusCommand command,
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
        typeof(AttendanceRegularizationStatusDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendanceRegularizationStatusByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AttendanceRegularizationStatusDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAttendanceRegularizationStatusesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAttendanceRegularizationStatusCommand command,
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
            new DeleteAttendanceRegularizationStatusCommand(id),
            cancellationToken);

        return NoContent();
    }
}
