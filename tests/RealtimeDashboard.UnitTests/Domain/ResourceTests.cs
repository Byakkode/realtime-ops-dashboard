using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Domain.Events;

namespace RealtimeDashboard.UnitTests.Domain;

public class ResourceTests
{
    [Fact]
    public void Create_WithValidData_ShouldInitializeCorrectly()
    {
        var resource = Resource.Create("Bed 101", "ICU bed", "Bed", "Ward A");

        Assert.Equal("Bed 101", resource.Name);
        Assert.Equal(ResourceStatus.Available, resource.Status);
        Assert.Empty(resource.DomainEvents);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            Resource.Create("", "description", "Bed"));
    }

    [Fact]
    public void ChangeStatus_ToDifferentStatus_ShouldRaiseDomainEvent()
    {
        var resource = Resource.Create("Bed 101", "ICU bed", "Bed");

        resource.ChangeStatus(ResourceStatus.Occupied, "operator@hospital.com");

        Assert.Equal(ResourceStatus.Occupied, resource.Status);
        Assert.Single(resource.DomainEvents);

        var domainEvent = resource.DomainEvents.First() as ResourceStatusChangedEvent;
        Assert.NotNull(domainEvent);
        Assert.Equal(ResourceStatus.Available, domainEvent.PreviousStatus);
        Assert.Equal(ResourceStatus.Occupied, domainEvent.NewStatus);
        Assert.Equal("operator@hospital.com", domainEvent.ChangedBy);
    }

    [Fact]
    public void ChangeStatus_ToSameStatus_ShouldNotRaiseDomainEvent()
    {
        var resource = Resource.Create("Bed 101", "ICU bed", "Bed");

        resource.ChangeStatus(ResourceStatus.Available, "operator@hospital.com");

        Assert.Empty(resource.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_AfterStatusChange_ShouldEmptyEvents()
    {
        var resource = Resource.Create("Bed 101", "ICU bed", "Bed");
        resource.ChangeStatus(ResourceStatus.Occupied, "operator@hospital.com");

        resource.ClearDomainEvents();

        Assert.Empty(resource.DomainEvents);
    }
}