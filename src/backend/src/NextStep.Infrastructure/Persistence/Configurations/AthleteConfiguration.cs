using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Infrastructure.Persistence.Configurations;

public class AthleteConfiguration : IEntityTypeConfiguration<Athlete>
{
    public void Configure(EntityTypeBuilder<Athlete> builder)
    {
        builder.ToTable("Athletes");

        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.PersonalInfo, pi =>
        {
            pi.Property(p => p.Name)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();

            pi.Property(p => p.BirthDate)
                .HasColumnName("BirthDate")
                .IsRequired();
        });

        builder.OwnsOne(a => a.PhysiologicalData, pd =>
        {
            pd.Property(p => p.MaxHeartRate)
                .HasColumnName("MaxHeartRate");

            pd.Property(p => p.LactateThresholdHeartRate)
                .HasColumnName("LactateThresholdHeartRate");

            pd.Property(p => p.LactateThresholdPace)
                .HasColumnName("LactateThresholdPace");
        });

        builder.OwnsOne(a => a.TrainingAccess, ta =>
        {
            ta.Property(t => t.HasTrackAccess)
                .HasColumnName("HasTrackAccess")
                .HasDefaultValue(false);
        });

        // HeartRateZones and PaceZones are calculated dynamically from LactateThreshold values
        // They are not stored in the database
        builder.Ignore(a => a.HeartRateZones);
        builder.Ignore(a => a.PaceZones);
    }
}
