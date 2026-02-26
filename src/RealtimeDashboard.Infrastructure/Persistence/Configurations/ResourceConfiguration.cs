using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Infrastructure.Persistence.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(r => r.Description)
            .HasMaxLength(500);
        
        builder.Property(r => r.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Location)
            .HasMaxLength(100);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.HasMany(r => r.Thresholds)
            .WithOne()
            .HasForeignKey(t => t.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Ignore(r => r.DomainEvents);
        
        builder.ToTable("Resource");
    }
}