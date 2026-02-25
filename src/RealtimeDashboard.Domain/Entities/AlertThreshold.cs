using RealtimeDashboard.Domain.Common;
using RealtimeDashboard.Domain.Enums;

namespace RealtimeDashboard.Domain.Entities;

public class AlertThreshold : BaseEntity
{
    public Guid ResourceId { get; private set; }
    public ResourceStatus TriggerOnStatus { get; private set; }
    public AlertLevel Level  { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    
    private AlertThreshold() { }

    public static AlertThreshold Create(Guid resourceId, ResourceStatus triggerOnStatus, AlertLevel level,
        string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        return new AlertThreshold
        {
            ResourceId = resourceId,
            TriggerOnStatus = triggerOnStatus,
            Level = level,
            Message = message,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}