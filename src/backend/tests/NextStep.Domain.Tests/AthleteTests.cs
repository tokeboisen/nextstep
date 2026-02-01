using FluentAssertions;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Domain.Tests;

public class AthleteTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateAthlete()
    {
        // Arrange
        var name = "John Doe";
        var birthDate = new DateOnly(1990, 5, 15);

        // Act
        var athlete = Athlete.Create(name, birthDate);

        // Assert
        athlete.Should().NotBeNull();
        athlete.Id.Should().NotBeEmpty();
        athlete.PersonalInfo.Name.Should().Be(name);
        athlete.PersonalInfo.BirthDate.Should().Be(birthDate);
        athlete.PhysiologicalData.MaxHeartRate.Should().BeNull();
        athlete.TrainingAccess.HasTrackAccess.Should().BeFalse();
    }

    [Fact]
    public void UpdatePersonalInfo_ShouldUpdateNameAndBirthDate()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var newName = "Jane Doe";
        var newBirthDate = new DateOnly(1985, 3, 20);

        // Act
        athlete.UpdatePersonalInfo(newName, newBirthDate);

        // Assert
        athlete.PersonalInfo.Name.Should().Be(newName);
        athlete.PersonalInfo.BirthDate.Should().Be(newBirthDate);
    }

    [Fact]
    public void UpdatePhysiologicalData_ShouldUpdateHeartRateData()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        athlete.UpdatePhysiologicalData(185, 165, TimeSpan.FromMinutes(4.5));

        // Assert
        athlete.PhysiologicalData.MaxHeartRate.Should().Be(185);
        athlete.PhysiologicalData.LactateThresholdHeartRate.Should().Be(165);
        athlete.PhysiologicalData.LactateThresholdPace.Should().Be(TimeSpan.FromMinutes(4.5));
    }

    [Fact]
    public void UpdateTrainingAccess_ShouldUpdateTrackAccess()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        athlete.UpdateTrainingAccess(true);

        // Assert
        athlete.TrainingAccess.HasTrackAccess.Should().BeTrue();
    }

    [Fact]
    public void HeartRateZones_WithoutLactateThreshold_ShouldBeEmpty()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act & Assert
        athlete.HeartRateZones.Should().BeEmpty();
    }

    [Fact]
    public void HeartRateZones_WithLactateThreshold_ShouldBeCalculated()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        athlete.UpdatePhysiologicalData(185, 165, null);

        // Assert
        athlete.HeartRateZones.Should().HaveCount(5);
        athlete.HeartRateZones[0].Name.Should().Be("Recovery");
        athlete.HeartRateZones[4].Name.Should().Be("VO2max");
    }

    [Fact]
    public void PaceZones_WithoutLactateThreshold_ShouldBeEmpty()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act & Assert
        athlete.PaceZones.Should().BeEmpty();
    }

    [Fact]
    public void PaceZones_WithLactateThreshold_ShouldBeCalculated()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        athlete.UpdatePhysiologicalData(null, null, TimeSpan.FromMinutes(5));

        // Assert
        athlete.PaceZones.Should().HaveCount(5);
        athlete.PaceZones[0].Name.Should().Be("Recovery");
        athlete.PaceZones[4].Name.Should().Be("VO2max");
    }

    [Fact]
    public void Zones_ShouldRecalculateWhenPhysiologicalDataChanges()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        athlete.UpdatePhysiologicalData(185, 160, TimeSpan.FromMinutes(5));
        var originalZone1MaxBpm = athlete.HeartRateZones[0].MaxBpm;

        // Act
        athlete.UpdatePhysiologicalData(185, 170, TimeSpan.FromMinutes(5));

        // Assert
        athlete.HeartRateZones[0].MaxBpm.Should().NotBe(originalZone1MaxBpm);
    }
}
