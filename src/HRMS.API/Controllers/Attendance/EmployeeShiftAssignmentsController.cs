using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.CreateEmployeeShiftAssignment;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.DeleteEmployeeShiftAssignment;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Commands.UpdateEmployeeShiftAssignment;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.DTOs;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignmentById;
using HRMS.Modules.Attendance.Application.Features.EmployeeShiftAssignments.Queries.GetEmployeeShiftAssignments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance;

[ApiController]
[Route("api/employee-shift-assignments")]
public class EmployeeShiftAssignmentsController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeShiftAssignmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeShiftAssignmentCommand command,
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
        typeof(EmployeeShiftAssignmentDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeShiftAssignmentByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<EmployeeShiftAssignmentDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeShiftAssignmentsQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeShiftAssignmentCommand command,
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
            new DeleteEmployeeShiftAssignmentCommand(id),
            cancellationToken);

        return NoContent();
    }
}
