using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealtimeDashboard.Application.Alerts.Commands.ResolveAlert;
using RealtimeDashboard.Application.Alerts.Queries.GetActiveAlerts;

namespace RealtimeDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlertsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlertsController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>
    /// Returns all unresolved alerts, ordered by severity then date.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<AlertDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveAlertsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Resolves an alert by marking it as handled.
    /// </summary>
    [HttpPatch("{id:guid}/resolve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resolve(
        Guid id,
        [FromBody] ResolveAlertRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ResolveAlertCommand(id, request.ResolvedBy),
            cancellationToken);

        return NoContent();
    }
}

public record ResolveAlertRequest(string ResolvedBy);