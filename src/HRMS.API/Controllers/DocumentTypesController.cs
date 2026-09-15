using HRMS.BuildingBlocks.Application.Pagination;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.CreateDocumentType;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.DeleteDocumentType;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.Commands.UpdateDocumentType;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypeById;
using HRMS.Modules.Foundation.Application.Features.DocumentTypes.Queries.GetDocumentTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDocumentTypeCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            command,
            cancellationToken);

        return Created(
            $"/api/DocumentTypes/{id}",
            id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetDocumentTypeByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetDocumentTypesQuery(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDocumentTypeCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route id and command id must match.");
        }

        await _mediator.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteDocumentTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
}
