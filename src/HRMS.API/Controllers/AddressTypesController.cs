using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.CreateAddressType;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.DeleteAddressType;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.Commands.UpdateAddressType;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypeById;
using HRMS.Modules.Foundation.Application.Features.AddressTypes.Queries.GetAddressTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAddressTypeCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);

        return Created($"/api/AddressTypes/{id}", id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAddressTypeByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAddressTypesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateAddressTypeCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route id and command id must match.");

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteAddressTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
}
