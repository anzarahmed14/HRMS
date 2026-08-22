using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.CreateAttendanceRawLog;
using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.ImportAttendanceRawLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/attendance-raw-logs")]
public class AttendanceRawLogsController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceRawLogsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRawLogCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> Import(
    [FromBody] ImportAttendanceRawLogsCommand command,
    CancellationToken cancellationToken)
    {
        var count = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            count);
    }
}
