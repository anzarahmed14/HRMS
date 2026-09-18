using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.CreateAttendanceRawLog;
using HRMS.Modules.Attendance.Application.Features.AttendanceRawLogs.Commands.ImportAttendanceRawLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/[controller]")]
public class AttendanceRawLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceRawLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRawLogCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
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
        var count = await _mediator.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            count);
    }
}
