using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.CreateAttendanceShift;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.DeleteAttendanceShift;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Commands.UpdateAttendanceShift;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShiftById;
using HRMS.Modules.Attendance.Application.Features.AttendanceShifts.Queries.GetAttendanceShifts;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/attendance-shifts")]
public class AttendanceShiftsController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceShiftsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceShiftCommand command,
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
        typeof(AttendanceShiftDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceShiftByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }


    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AttendanceShiftDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceShiftsQuery(request),
            cancellationToken);

        return Ok(result);
    }
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAttendanceShiftCommand command,
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
            new DeleteAttendanceShiftCommand(id),
            cancellationToken);

        return NoContent();
    }
}



