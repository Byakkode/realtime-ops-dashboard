using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Events;

namespace RealtimeDashboard.UnitTests.Domain;

public class AlertTests
{
    [Fact]
    public void Create_WithValidData_ShouldRaiseAlertTriggeredEvent()
    {
        var resourceId = Guid.NewGuid();

        var alert = Alert.Create(resourceId, "Bed 101", AlertLevel.Critical, "Bed occupied beyond threshold");

        Assert.False(alert.IsResolved);
        Assert.Single(alert.DomainEvents);

        var domainEvent = alert.DomainEvents.First() as AlertTriggeredEvent;
        Assert.NotNull(domainEvent);
        Assert.Equal(AlertLevel.Critical, domainEvent.Level);
    }

    [Fact]
    public void Resolve_UnresolvedAlert_ShouldMarkAsResolved()
    {
        var alert = Alert.Create(Guid.NewGuid(), "Bed 101", AlertLevel.Warning, "Test alert");
        alert.ClearDomainEvents();

        alert.Resolve("admin@hospital.com");

        Assert.True(alert.IsResolved);
        Assert.NotNull(alert.ResolvedAt);
        Assert.Equal("admin@hospital.com", alert.ResolvedBy);
    }

    [Fact]
    public void Resolve_AlreadyResolvedAlert_ShouldNotChangeResolvedAt()
    {
        var alert = Alert.Create(Guid.NewGuid(), "Bed 101", AlertLevel.Warning, "Test alert");
        alert.Resolve("admin@hospital.com");
        var firstResolvedAt = alert.ResolvedAt;

        alert.Resolve("another@hospital.com");

        Assert.Equal(firstResolvedAt, alert.ResolvedAt);
        Assert.Equal("admin@hospital.com", alert.ResolvedBy);
    }
}