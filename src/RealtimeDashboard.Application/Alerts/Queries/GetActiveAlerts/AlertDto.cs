using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Application.Alerts.Queries.GetActiveAlerts;

public record AlertDto(
    Guid Id,
    Guid ResourceId,
    string ResourceName,
    AlertLevel Level,
    string Message,
    bool IsResolved,
    DateTime? ResolvedAt,
    string? ResolvedBy,
    DateTime CreatedAt
);