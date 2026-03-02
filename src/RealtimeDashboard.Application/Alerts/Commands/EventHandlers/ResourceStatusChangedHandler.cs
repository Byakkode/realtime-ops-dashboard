using MediatR;
using RealtimeDashboard.Application.Common.Interfaces;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Events;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Alerts.EventHandlers;

public class ResourceStatusChangedHandler : INotificationHandler<ResourceStatusChangedEvent>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRealtimeNotifier _notifier;

    public ResourceStatusChangedHandler(
        IResourceRepository resourceRepository,
        IAlertRepository alertRepository,
        IUnitOfWork unitOfWork,
        IRealtimeNotifier notifier)
    {
        _resourceRepository = resourceRepository;
        _alertRepository = alertRepository;
        _unitOfWork = unitOfWork;
        _notifier = notifier;
    }

    public async Task Handle(
        ResourceStatusChangedEvent notification,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository
            .GetByIdAsync(notification.ResourceId, cancellationToken);

        if (resource is null)
        {
            return;
        }

        await _notifier.NotifyResourceStatusChangedAsync(
            notification.ResourceId,
            notification.ResourceName,
            notification.PreviousStatus,
            notification.NewStatus,
            notification.ChangedBy,
            cancellationToken
        );

        var matchingThreshold = resource.Thresholds
            .FirstOrDefault(t => t.TriggerOnStatus == notification.NewStatus
                              && t.IsActive);

        if (matchingThreshold is null) return;

        var alert = Alert.Create(
            resourceId: resource.Id,
            resourceName: resource.Name,
            level: matchingThreshold.Level,
            message: matchingThreshold.Message
        );

        await _alertRepository.AddAsync(alert, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _notifier.NotifyAlertCreatedAsync(
            alert.Id,
            alert.ResourceId,
            alert.ResourceName,
            alert.Level,
            alert.Message,
            cancellationToken
        );

        alert.ClearDomainEvents();
    }
}