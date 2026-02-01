using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextStep.Domain.AthleteProfile.Entities;
using NextStep.Domain.AthleteProfile.ValueObjects;

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

            pd.Property(p => p.LactateThreshold)
                .HasColumnName("LactateThreshold");
        });

        builder.OwnsOne(a => a.TrainingAccess, ta =>
        {
            ta.Property(t => t.HasTrackAccess)
                .HasColumnName("HasTrackAccess")
                .HasDefaultValue(false);
        });

        builder.OwnsMany(a => a.HeartRateZones, hrz =>
        {
            hrz.ToTable("HeartRateZones");

            hrz.WithOwner().HasForeignKey("AthleteId");

            hrz.Property<int>("Id")
                .ValueGeneratedOnAdd();

            hrz.HasKey("Id");

            hrz.Property(z => z.ZoneNumber)
                .HasColumnName("ZoneNumber")
                .IsRequired();

            hrz.Property(z => z.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsRequired();

            hrz.Property(z => z.MinBpm)
                .HasColumnName("MinBpm")
                .IsRequired();

            hrz.Property(z => z.MaxBpm)
                .HasColumnName("MaxBpm")
                .IsRequired();
        });

        builder.OwnsMany(a => a.PaceZones, pz =>
        {
            pz.ToTable("PaceZones");

            pz.WithOwner().HasForeignKey("AthleteId");

            pz.Property<int>("Id")
                .ValueGeneratedOnAdd();

            pz.HasKey("Id");

            pz.Property(z => z.ZoneNumber)
                .HasColumnName("ZoneNumber")
                .IsRequired();

            pz.Property(z => z.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsRequired();

            pz.Property(z => z.MinPacePerKm)
                .HasColumnName("MinPacePerKm")
                .IsRequired();

            pz.Property(z => z.MaxPacePerKm)
                .HasColumnName("MaxPacePerKm")
                .IsRequired();
        });
    }
}
