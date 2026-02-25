using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Domain.Events;

public sealed record ResourceStatusChangedEvent(
    Guid ResourceId,
    string ResourceName,
    ResourceStatus PreviousStatus,
    ResourceStatus NewStatus,
    string ChangedBy,
    DateTime OccurredAt
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}