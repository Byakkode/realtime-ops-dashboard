using RealtimeDashboard.Domain.Common;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Events;

namespace RealtimeDashboard.Domain.Entities;

public class Resource : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public ResourceStatus Status { get; private set; }
    public string? Location { get; private set; }

    private readonly List<AlertThreshold> _thresholds = [];
    public IReadOnlyCollection<AlertThreshold> Thresholds => _thresholds.AsReadOnly();
    
    private Resource() { }

    public static Resource Create(string name, string description, string category, string? location = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(category);

        var resource = new Resource
        {
            Name = name,
            Description = description,
            Category = category,
            Location = location,
            Status = ResourceStatus.Available
        };
        
        return resource;
    }

    public void ChangeStatus(ResourceStatus newStatus, string changedBy)
    {
        if (Status == newStatus)
            return;
        
        var previousStatus = Status;
        Status = newStatus;
        SetUpdatedAt();
        
        AddDomainEvent(new ResourceStatusChangedEvent(
            ResourceId: Id,
            ResourceName: Name,
            PreviousStatus: previousStatus,
            NewStatus: newStatus,
            ChangedBy: changedBy,
            OccurredAt: DateTime.UtcNow
        ));
    }

    public void AddThreshold(AlertThreshold threshold)
    {
        ArgumentNullException.ThrowIfNull(threshold);
        _thresholds.Add(threshold);
    }

    public void UpdateDetails(string name, string description, string? location)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        Location = location;
        SetUpdatedAt();
    }
}