using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Application.Common.Interfaces;

public interface IRealtimeNotifier
{
    Task NotifyResourceStatusChangedAsync(
        Guid resourceId,
        string resourceName,
        ResourceStatus previousStatus,
        ResourceStatus newStatus,
        string changedBy,
        CancellationToken cancellationToken = default);

    Task NotifyAlertCreatedAsync(
        Guid alertId,
        Guid resourceId,
        string resourceName,
        AlertLevel level,
        string message,
        CancellationToken cancellationToken = default);
}