using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Domain.Events;

public sealed record AlertTriggeredEvent(
    Guid AlertId,
    Guid ResourceId,
    string ResourceName,
    AlertLevel Level,
    string Message,
    DateTime OccurredAt
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}