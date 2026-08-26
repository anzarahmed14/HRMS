using HRMS.Application.Features.EmploymentTypes.Commands.CreateEmploymentType;
using HRMS.Application.Features.EmploymentTypes.Commands.DeleteEmploymentType;
using HRMS.Application.Features.EmploymentTypes.Commands.UpdateEmploymentType;
using HRMS.Application.Features.EmploymentTypes.DTOs;
using HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypeById;
using HRMS.Application.Features.EmploymentTypes.Queries.GetEmploymentTypes;
using HRMS.BuildingBlocks.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee;

[ApiController]
[Route("api/[controller]")]
public class EmploymentTypesController : ControllerBase
{
    private readonly ISender _sender;

    public EmploymentTypesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmploymentTypeCommand command,
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
        typeof(EmploymentTypeDto),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmploymentTypeByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<EmploymentTypeDto>),
        StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetEmploymentTypesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmploymentTypeCommand command,
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
            new DeleteEmploymentTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
}
