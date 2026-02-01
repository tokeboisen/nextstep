using FluentAssertions;
using NextStep.Domain.AthleteProfile.Entities;
using NextStep.Domain.AthleteProfile.ValueObjects;

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
        athlete.UpdatePhysiologicalData(185, 165);

        // Assert
        athlete.PhysiologicalData.MaxHeartRate.Should().Be(185);
        athlete.PhysiologicalData.LactateThreshold.Should().Be(165);
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
    public void SetHeartRateZones_WithValidZones_ShouldSetZones()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var zones = new[]
        {
            HeartRateZone.Create(1, "Recovery", 100, 120),
            HeartRateZone.Create(2, "Aerobic", 120, 140),
        };

        // Act
        athlete.SetHeartRateZones(zones);

        // Assert
        athlete.HeartRateZones.Should().HaveCount(2);
        athlete.HeartRateZones[0].ZoneNumber.Should().Be(1);
        athlete.HeartRateZones[1].ZoneNumber.Should().Be(2);
    }

    [Fact]
    public void SetHeartRateZones_WithDuplicateZoneNumbers_ShouldThrow()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var zones = new[]
        {
            HeartRateZone.Create(1, "Recovery", 100, 120),
            HeartRateZone.Create(1, "Also Recovery", 110, 130),
        };

        // Act
        var act = () => athlete.SetHeartRateZones(zones);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Duplicate*");
    }

    [Fact]
    public void SetPaceZones_WithValidZones_ShouldSetZones()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var zones = new[]
        {
            PaceZone.Create(1, "Easy", TimeSpan.FromMinutes(6), TimeSpan.FromMinutes(7)),
            PaceZone.Create(2, "Tempo", TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(6)),
        };

        // Act
        athlete.SetPaceZones(zones);

        // Assert
        athlete.PaceZones.Should().HaveCount(2);
    }
}
