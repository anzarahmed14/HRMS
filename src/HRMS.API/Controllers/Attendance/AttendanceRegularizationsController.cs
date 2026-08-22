using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.ApproveAttendanceRegularization;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.CreateAttendanceRegularization;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.DeleteAttendanceRegularization;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.RejectAttendanceRegularization;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Commands.UpdateAttendanceRegularization;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizationById;
using HRMS.Modules.Attendance.Application.Features.AttendanceRegularizations.Queries.GetAttendanceRegularizations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/attendance-regularizations")]
public class AttendanceRegularizationsController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceRegularizationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAttendanceRegularizationCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            id);
    }
    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(
    Guid id,
    [FromBody] ApproveAttendanceRegularizationRequest request,
    CancellationToken cancellationToken)
    {
        var command =
            new ApproveAttendanceRegularizationCommand(
                id,
                request.Remarks);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
    Guid id,
    [FromBody] RejectAttendanceRegularizationRequest request,
    CancellationToken cancellationToken)
    {
        var command =
            new RejectAttendanceRegularizationCommand(
                id,
                request.Remarks);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
    Guid id,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAttendanceRegularizationByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] Guid? employeeId,
    [FromQuery] DateOnly? fromDate,
    [FromQuery] DateOnly? toDate,
    [FromQuery] Guid? statusId,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var query = new GetAttendanceRegularizationsQuery(
            employeeId,
            fromDate,
            toDate,
            statusId,
            pageNumber,
            pageSize);

        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateAttendanceRegularizationRequest request,
    CancellationToken cancellationToken)
    {
        var command =
            new UpdateAttendanceRegularizationCommand(
                id,
                request.RequestedCheckIn,
                request.RequestedCheckOut,
                request.Reason);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteAttendanceRegularizationCommand(id),
            cancellationToken);

        return NoContent();
    }
}
