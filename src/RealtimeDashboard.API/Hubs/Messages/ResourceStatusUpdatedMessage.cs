using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.API.Hubs.Messages;

public record ResourceStatusUpdatedMessage(
    Guid ResourceId,
    string ResourceName,
    string PreviousStatus,
    string NewStatus,
    string ChangedBy,
    DateTime OccurredAt
);