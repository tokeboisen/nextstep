using FluentAssertions;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Domain.Tests;

public class PersonalInfoTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreatePersonalInfo()
    {
        // Arrange & Act
        var personalInfo = PersonalInfo.Create("John Doe", new DateOnly(1990, 5, 15));

        // Assert
        personalInfo.Name.Should().Be("John Doe");
        personalInfo.BirthDate.Should().Be(new DateOnly(1990, 5, 15));
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PersonalInfo.Create("", new DateOnly(1990, 5, 15));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Name*empty*");
    }

    [Fact]
    public void Create_WithFutureBirthDate_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PersonalInfo.Create("John", DateOnly.FromDateTime(DateTime.Today.AddDays(1)));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*future*");
    }

    [Fact]
    public void CalculateAge_ShouldReturnCorrectAge()
    {
        // Arrange
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));
        var personalInfo = PersonalInfo.Create("John", birthDate);

        // Act
        var age = personalInfo.CalculateAge();

        // Assert
        age.Should().Be(30);
    }
}

public class PhysiologicalDataTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreatePhysiologicalData()
    {
        // Arrange & Act
        var data = PhysiologicalData.Create(185, 165);

        // Assert
        data.MaxHeartRate.Should().Be(185);
        data.LactateThreshold.Should().Be(165);
    }

    [Fact]
    public void Create_WithNullValues_ShouldAllowNulls()
    {
        // Arrange & Act
        var data = PhysiologicalData.Create(null, null);

        // Assert
        data.MaxHeartRate.Should().BeNull();
        data.LactateThreshold.Should().BeNull();
    }

    [Fact]
    public void Create_WithMaxHeartRateTooLow_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(50, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Max heart rate*");
    }

    [Fact]
    public void Create_WithLactateThresholdHigherThanMax_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(180, 190);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*exceed*");
    }
}

public class HeartRateZoneTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateZone()
    {
        // Arrange & Act
        var zone = HeartRateZone.Create(1, "Recovery", 100, 120);

        // Assert
        zone.ZoneNumber.Should().Be(1);
        zone.Name.Should().Be("Recovery");
        zone.MinBpm.Should().Be(100);
        zone.MaxBpm.Should().Be(120);
    }

    [Fact]
    public void Create_WithInvalidZoneNumber_ShouldThrow()
    {
        // Arrange & Act
        var act = () => HeartRateZone.Create(0, "Invalid", 100, 120);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Zone number*");
    }

    [Fact]
    public void Create_WithMinGreaterThanMax_ShouldThrow()
    {
        // Arrange & Act
        var act = () => HeartRateZone.Create(1, "Invalid", 150, 100);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*less than*");
    }
}

public class PaceZoneTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateZone()
    {
        // Arrange & Act
        var zone = PaceZone.Create(1, "Easy", TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(6));

        // Assert
        zone.ZoneNumber.Should().Be(1);
        zone.Name.Should().Be("Easy");
        zone.MinPacePerKm.Should().Be(TimeSpan.FromMinutes(5));
        zone.MaxPacePerKm.Should().Be(TimeSpan.FromMinutes(6));
    }

    [Fact]
    public void FormatMinPace_ShouldReturnFormattedString()
    {
        // Arrange
        var zone = PaceZone.Create(1, "Easy", new TimeSpan(0, 5, 30), new TimeSpan(0, 6, 0));

        // Act
        var formatted = zone.FormatMinPace();

        // Assert
        formatted.Should().Be("5:30");
    }
}

public class TrainingAccessTests
{
    [Fact]
    public void Create_WithTrue_ShouldSetHasTrackAccess()
    {
        // Arrange & Act
        var access = TrainingAccess.Create(true);

        // Assert
        access.HasTrackAccess.Should().BeTrue();
    }

    [Fact]
    public void Default_ShouldReturnFalse()
    {
        // Arrange & Act
        var access = TrainingAccess.Default();

        // Assert
        access.HasTrackAccess.Should().BeFalse();
    }
}
