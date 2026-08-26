using HRMS.Application.Features.EmployeeAddresses.Commands.CreateEmployeeAddress;
using HRMS.Application.Features.EmployeeAddresses.Commands.DeleteEmployeeAddress;
using HRMS.Application.Features.EmployeeAddresses.Commands.UpdateEmployeeAddress;
using HRMS.Application.Features.EmployeeAddresses.DTOs;
using HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddressById;
using HRMS.Application.Features.EmployeeAddresses.Queries.GetEmployeeAddresses;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class EmployeeAddressesController : ControllerBase
{
    private readonly ISender _sender;

    public EmployeeAddressesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeAddressCommand command,
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
        typeof(EmployeeAddressDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeAddressByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(
        typeof(PagedResult<EmployeeAddressDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        Guid employeeId,
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmployeeAddressesQuery(
                employeeId,
                request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeAddressCommand command,
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
            new DeleteEmployeeAddressCommand(id),
            cancellationToken);

        return NoContent();
    }
}
