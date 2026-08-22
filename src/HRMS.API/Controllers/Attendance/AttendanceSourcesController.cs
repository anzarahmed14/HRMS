using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.CreateAttendanceSource;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.DeleteAttendanceSource;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.Commands.UpdateAttendanceSource;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSourceById;
using HRMS.Modules.Attendance.Application.Features.AttendanceSources.Queries.GetAttendanceSources;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/attendance-sources")]
public class AttendanceSourcesController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceSourcesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceSourceCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AttendanceSourceDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceSourceByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AttendanceSourceDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceSourcesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAttendanceSourceCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        await _sender.Send(
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
        await _sender.Send(
            new DeleteAttendanceSourceCommand(id),
            cancellationToken);

        return NoContent();
    }
}
