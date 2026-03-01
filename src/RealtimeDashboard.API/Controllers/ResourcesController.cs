using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealtimeDashboard.Application.Resources.Commands.ChangeResourceStatus;
using RealtimeDashboard.Application.Resources.Commands.CreateResource;
using RealtimeDashboard.Application.Resources.Queries.GetAllResources;
using RealtimeDashboard.Application.Resources.Queries.GetResourceById;

namespace RealtimeDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ResourcesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourcesController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>
    /// Returns all resources, optionally filtered by category.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ResourceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllResourcesQuery(category), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns a single resource by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResourceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetResourceByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new resource.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateResourceCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Changes the status of a resource.
    /// Triggers an alert if a matching threshold is configured.
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeResourceStatusRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeResourceStatusCommand(id, request.NewStatus, request.ChangedBy),
            cancellationToken);

        return NoContent();
    }
}

public record ChangeResourceStatusRequest(
    RealtimeDashboard.Domain.Enums.ResourceStatus NewStatus,
    string ChangedBy
);