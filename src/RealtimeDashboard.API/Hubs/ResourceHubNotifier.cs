using Microsoft.AspNetCore.SignalR;
using RealtimeDashboard.Application.Common.Interfaces;
using RealtimeDashboard.API.Hubs.Messages;
using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.API.Hubs;

public class ResourceHubNotifier : IRealtimeNotifier
{
    private readonly IHubContext<ResourceHub> _hubContext;

    public ResourceHubNotifier(IHubContext<ResourceHub> hubContext)
        => _hubContext = hubContext;

    public async Task NotifyResourceStatusChangedAsync(
        Guid resourceId,
        string resourceName,
        ResourceStatus previousStatus,
        ResourceStatus newStatus,
        string changedBy,
        CancellationToken cancellationToken = default)
    {
        var message = new ResourceStatusUpdatedMessage(
            resourceId,
            resourceName,
            previousStatus.ToString(),
            newStatus.ToString(),
            changedBy,
            DateTime.UtcNow
        );

        await _hubContext.Clients
            .Group("All")
            .SendAsync("ReceiveResourceUpdate", message, cancellationToken);
    }

    public async Task NotifyAlertCreatedAsync(
        Guid alertId,
        Guid resourceId,
        string resourceName,
        AlertLevel level,
        string message,
        CancellationToken cancellationToken = default)
    {
        var alertMessage = new AlertCreatedMessage(
            alertId,
            resourceId,
            resourceName,
            level.ToString(),
            message,
            DateTime.UtcNow
        );

        var target = level == AlertLevel.Critical
            ? _hubContext.Clients.Group("Admins")
            : _hubContext.Clients.Group("All");

        await target.SendAsync("ReceiveAlert", alertMessage, cancellationToken);
    }
}