using RealtimeDashboard.Domain.Common;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Events;

namespace RealtimeDashboard.Domain.Entities;

public class Alert : BaseEntity
{
    public Guid ResourceId { get; private set; }
    public string ResourceName { get; private set; } = string.Empty;
    public AlertLevel Level { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsResolved { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public string? ResolvedBy { get; private set; }
    
    private Alert() { }

    public static Alert Create(Guid resourceId, string resourceName, AlertLevel level, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        var alert = new Alert
        {
            ResourceId = resourceId,
            ResourceName = resourceName,
            Level = level,
            Message = message,
            IsResolved = false
        };
        
        alert.AddDomainEvent(new AlertTriggeredEvent(
            AlertId: alert.Id,
            ResourceId: resourceId,
            ResourceName: resourceName,
            Level: level,
            Message: message,
            OccurredAt: DateTime.UtcNow
        ));
        
        return alert;
    }

    public void Resolve(string resolvedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resolvedBy);
        
        if (IsResolved)
            return;
        
        IsResolved = true;
        ResolvedAt = DateTime.UtcNow;
        ResolvedBy = resolvedBy;
        SetUpdatedAt();
    }
}