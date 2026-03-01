using Microsoft.EntityFrameworkCore;
using RealtimeDashboard.Domain.Entities;
using RealtimeDashboard.Domain.Enums;
using RealtimeDashboard.Infrastructure.Persistence;

namespace RealtimeDashboard.API.SeedData;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Resources.AnyAsync()) return;

        var resources = new List<Resource>
        {
            Resource.Create("Bed 101", "Standard ICU bed", "Bed", "Ward A - Floor 1"),
            Resource.Create("Bed 102", "Standard ICU bed", "Bed", "Ward A - Floor 1"),
            Resource.Create("Bed 201", "Pediatric bed", "Bed", "Ward B - Floor 2"),
            Resource.Create("OR-1", "Operating Room 1", "Room", "Surgery Wing"),
            Resource.Create("OR-2", "Operating Room 2", "Room", "Surgery Wing"),
            Resource.Create("MRI-1", "MRI Scanner Unit 1", "Equipment", "Radiology"),
        };

        var bed101 = resources[0];
        bed101.AddThreshold(AlertThreshold.Create(
            bed101.Id, ResourceStatus.UnderMaintenance,
            AlertLevel.Warning, "Bed 101 is under maintenance — capacity reduced"));
        bed101.AddThreshold(AlertThreshold.Create(
            bed101.Id, ResourceStatus.OutOfService,
            AlertLevel.Critical, "Bed 101 is out of service — immediate action required"));

        var mri = resources[5];
        mri.AddThreshold(AlertThreshold.Create(
            mri.Id, ResourceStatus.UnderMaintenance,
            AlertLevel.Critical, "MRI-1 unavailable — reroute patients to MRI-2"));

        await context.Resources.AddRangeAsync(resources);
        await context.SaveChangesAsync();
    }
}