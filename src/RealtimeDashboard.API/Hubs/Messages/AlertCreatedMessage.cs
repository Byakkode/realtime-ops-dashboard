namespace RealtimeDashboard.API.Hubs.Messages;

public record AlertCreatedMessage(
    Guid AlertId,
    Guid ResourceId,
    string ResourceName,
    string Level,
    string Message,
    DateTime CreatedAt
);