using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Commands.UpdateEmployee;
using HRMS.Application.Features.Employees.Queries.GetEmployeeById;
using HRMS.Modules.Employee.Application.Features.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

//[Authorize(Roles = "Employee")]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "Employee.View")]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEmployeesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "Employee.View")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEmployeeByIdQuery(id),
            cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "Employee.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Employee.Update")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route id and command id must match.");

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Employee.Delete")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new Application.Features.Employees.Commands.DeleteEmployee.DeleteEmployeeCommand(id),
            cancellationToken);

        return NoContent();
    }
}