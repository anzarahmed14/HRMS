using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.CreateManualAttendanceRecord;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.ProcessAttendanceRecords;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Commands.UpdateAttendanceRecord;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.DTOs;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecordById;
using HRMS.Modules.Attendance.Application.Features.AttendanceRecords.Queries.GetAttendanceRecords;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/attendance-records")]
public class AttendanceRecordsController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceRecordsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("process")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> Process(
        [FromBody] ProcessAttendanceRecordsCommand command,
        CancellationToken cancellationToken)
    {
        var count = await _sender.Send(
            command,
            cancellationToken);

        return Ok(new
        {
            message = "Attendance records processed successfully.",
            createdRecords = count
        });
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(AttendanceRecordDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceRecordByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    [ProducesResponseType(
    typeof(PagedResult<AttendanceRecordDto>),
    StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
    [FromQuery] PagedRequest page,
    [FromQuery] Guid? employeeId,
    [FromQuery] DateOnly? fromDate,
    [FromQuery] DateOnly? toDate,
    [FromQuery] string? status,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceRecordsQuery(
                page,
                employeeId,
                fromDate,
                toDate,
                status),
            cancellationToken);

        return Ok(result);
    }
    [HttpPost("manual")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManual(
    [FromBody] CreateManualAttendanceRecordCommand command,
    CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateAttendanceRecordCommand command,
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
}
