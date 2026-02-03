using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.AthleteProfile.Entities;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("Goals");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.RaceDate)
            .IsRequired();

        builder.Property(g => g.TargetTime)
            .IsRequired();

        builder.Property(g => g.IsPrimary)
            .HasDefaultValue(false);

        builder.OwnsOne(g => g.Distance, d =>
        {
            d.Property(x => x.DistanceType)
                .HasColumnName("DistanceType")
                .HasDefaultValue(DistanceType.Distance5K);

            d.Property(x => x.CustomDistanceKm)
                .HasColumnName("CustomDistanceKm")
                .HasPrecision(10, 2);
        });

        // Relationship is configured in AthleteConfiguration
    }
}
