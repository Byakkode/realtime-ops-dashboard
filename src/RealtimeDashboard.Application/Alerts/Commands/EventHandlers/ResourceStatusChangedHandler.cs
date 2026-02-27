using MediatR;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Events;
using RealtimeDashboard.Domain.Interfaces;

namespace RealtimeDashboard.Application.Alerts.Commands.EventHandlers;

public class ResourceStatusChangedHandler : INotificationHandler<ResourceStatusChangedEvent>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResourceStatusChangedHandler(
        IResourceRepository resourceRepository,
        IAlertRepository alertRepository,
        IUnitOfWork unitOfWork)
    {
        _resourceRepository = resourceRepository;
        _alertRepository = alertRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ResourceStatusChangedEvent notification,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository
            .GetByIdAsync(notification.ResourceId, cancellationToken);

        if (resource is null) return;

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

        alert.ClearDomainEvents();
    }
}