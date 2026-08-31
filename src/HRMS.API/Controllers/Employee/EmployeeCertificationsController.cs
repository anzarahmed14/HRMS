using HRMS.Application.Features.Certifications.Commands.CreateEmployeeCertification;
using HRMS.Application.Features.Certifications.Commands.DeleteEmployeeCertification;
using HRMS.Application.Features.Certifications.Commands.UpdateEmployeeCertification;
using HRMS.Application.Features.Certifications.Commands.VerifyEmployeeCertification;
using HRMS.Application.Features.Certifications.DTOs;
using HRMS.Application.Features.Certifications.Queries.GetEmployeeCertificationById;
using HRMS.Application.Features.Certifications.Queries.GetEmployeeCertifications;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class EmployeeCertificationsController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeCertificationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeCertificationCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeCertificationByIdQuery(id),
            cancellationToken);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{employeeId:guid}")]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeCertificationsQuery(employeeId, request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeCertificationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route ID does not match request ID.");

        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/verify")]
    public async Task<IActionResult> Verify(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new VerifyEmployeeCertificationCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteEmployeeCertificationCommand(id),
            cancellationToken);

        return NoContent();
    }
}
