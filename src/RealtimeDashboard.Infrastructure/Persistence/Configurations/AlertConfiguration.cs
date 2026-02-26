using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Infrastructure.Persistence.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.ResourceName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Level)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(a => a.Message)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.ResolvedBy)
            .HasMaxLength(100);

        builder.HasIndex(a => a.IsResolved);
        builder.HasIndex(a => a.ResourceId);
        
        builder.Ignore(a => a.DomainEvents);
        
        builder.ToTable("Alerts");
    }
}