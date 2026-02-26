using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealtimeDashboard.Domain.Entities;

namespace RealtimeDashboard.Infrastructure.Persistence.Configurations;

public class AlertThresholdConfiguration : IEntityTypeConfiguration<AlertThreshold>
{
    public void Configure(EntityTypeBuilder<AlertThreshold> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TriggerOnStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Level)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Message)
            .IsRequired()
            .HasMaxLength(200);

        builder.Ignore(t => t.DomainEvents);

        builder.ToTable("AlertThresholds");
    }
}