using MediatR;

namespace RealtimeDashboard.Domain.Events;

public interface IDomainEvent  : INotification
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}