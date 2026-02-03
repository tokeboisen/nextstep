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
        var data = PhysiologicalData.Create(185, 165, TimeSpan.FromMinutes(4.5));

        // Assert
        data.MaxHeartRate.Should().Be(185);
        data.LactateThresholdHeartRate.Should().Be(165);
        data.LactateThresholdPace.Should().Be(TimeSpan.FromMinutes(4.5));
    }

    [Fact]
    public void Create_WithNullValues_ShouldAllowNulls()
    {
        // Arrange & Act
        var data = PhysiologicalData.Create(null, null, null);

        // Assert
        data.MaxHeartRate.Should().BeNull();
        data.LactateThresholdHeartRate.Should().BeNull();
        data.LactateThresholdPace.Should().BeNull();
    }

    [Fact]
    public void Create_WithMaxHeartRateTooLow_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(50, null, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Max heart rate*");
    }

    [Fact]
    public void Create_WithLactateThresholdHigherThanMax_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(180, 190, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*exceed*");
    }

    [Fact]
    public void Create_WithPaceTooFast_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(null, null, TimeSpan.FromMinutes(1.5));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Lactate threshold pace*");
    }

    [Fact]
    public void Create_WithPaceTooSlow_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PhysiologicalData.Create(null, null, TimeSpan.FromMinutes(12));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Lactate threshold pace*");
    }
}

public class HeartRateZoneTests
{
    [Fact]
    public void CalculateFromLactateThreshold_WithValidLTHR_ShouldReturn5Zones()
    {
        // Arrange & Act
        var zones = HeartRateZone.CalculateFromLactateThreshold(165);

        // Assert
        zones.Should().HaveCount(5);
        zones[0].ZoneNumber.Should().Be(1);
        zones[0].Name.Should().Be("Recovery");
        zones[4].ZoneNumber.Should().Be(5);
        zones[4].Name.Should().Be("VO2max");
    }

    [Fact]
    public void CalculateFromLactateThreshold_ShouldCalculateCorrectBpmRanges()
    {
        // Arrange
        var lthr = 170;

        // Act
        var zones = HeartRateZone.CalculateFromLactateThreshold(lthr);

        // Assert
        // Zone 1 (Recovery): 50-80% of LTHR
        zones[0].MinBpm.Should().Be(85);  // 170 * 0.50
        zones[0].MaxBpm.Should().Be(136); // 170 * 0.80

        // Zone 4 (Threshold): 96-100% of LTHR
        zones[3].MinBpm.Should().Be(163); // 170 * 0.96
        zones[3].MaxBpm.Should().Be(170); // 170 * 1.00
    }

    [Fact]
    public void CalculateFromLactateThreshold_WithInvalidLTHR_ShouldThrow()
    {
        // Arrange & Act
        var act = () => HeartRateZone.CalculateFromLactateThreshold(50);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Lactate threshold heart rate*");
    }
}

public class PaceZoneTests
{
    [Fact]
    public void CalculateFromLactateThreshold_WithValidPace_ShouldReturn5Zones()
    {
        // Arrange & Act
        var zones = PaceZone.CalculateFromLactateThreshold(TimeSpan.FromMinutes(4.5));

        // Assert
        zones.Should().HaveCount(5);
        zones[0].ZoneNumber.Should().Be(1);
        zones[0].Name.Should().Be("Recovery");
        zones[4].ZoneNumber.Should().Be(5);
        zones[4].Name.Should().Be("VO2max");
    }

    [Fact]
    public void CalculateFromLactateThreshold_ShouldCalculateCorrectPaceRanges()
    {
        // Arrange
        var ltp = TimeSpan.FromMinutes(5); // 5:00 min/km

        // Act
        var zones = PaceZone.CalculateFromLactateThreshold(ltp);

        // Assert
        // Zone 1 (Recovery): 129-150% of LTP (slowest)
        zones[0].MinPacePerKm.Should().BeCloseTo(TimeSpan.FromSeconds(300 * 1.29), TimeSpan.FromSeconds(1));
        zones[0].MaxPacePerKm.Should().BeCloseTo(TimeSpan.FromSeconds(300 * 1.50), TimeSpan.FromSeconds(1));

        // Zone 4 (Threshold): 99-105% of LTP
        zones[3].MinPacePerKm.Should().BeCloseTo(TimeSpan.FromSeconds(300 * 0.99), TimeSpan.FromSeconds(1));
        zones[3].MaxPacePerKm.Should().BeCloseTo(TimeSpan.FromSeconds(300 * 1.05), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CalculateFromLactateThreshold_WithInvalidPace_ShouldThrow()
    {
        // Arrange & Act
        var act = () => PaceZone.CalculateFromLactateThreshold(TimeSpan.FromMinutes(1));

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Lactate threshold pace*");
    }

    [Fact]
    public void FormatMinPace_ShouldReturnFormattedString()
    {
        // Arrange
        var zones = PaceZone.CalculateFromLactateThreshold(TimeSpan.FromMinutes(5));

        // Act
        var formatted = zones[3].FormatMinPace();

        // Assert
        formatted.Should().MatchRegex(@"\d+:\d{2}");
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

public class WorkoutTypeTests
{
    [Theory]
    [InlineData(WorkoutType.CrossHIIT, true)]
    [InlineData(WorkoutType.Speed, true)]
    [InlineData(WorkoutType.TempoRun, true)]
    [InlineData(WorkoutType.LongRun, true)]
    [InlineData(WorkoutType.Rest, false)]
    [InlineData(WorkoutType.Recovery, false)]
    [InlineData(WorkoutType.EasyRun, false)]
    public void IsQualityWorkout_ShouldReturnCorrectValue(WorkoutType workoutType, bool expected)
    {
        // Act & Assert
        workoutType.IsQualityWorkout().Should().Be(expected);
    }

    [Theory]
    [InlineData(WorkoutType.Rest, true)]
    [InlineData(WorkoutType.Recovery, true)]
    [InlineData(WorkoutType.EasyRun, true)]
    [InlineData(WorkoutType.CrossHIIT, false)]
    [InlineData(WorkoutType.Speed, false)]
    [InlineData(WorkoutType.TempoRun, false)]
    [InlineData(WorkoutType.LongRun, false)]
    public void IsEasyWorkout_ShouldReturnCorrectValue(WorkoutType workoutType, bool expected)
    {
        // Act & Assert
        workoutType.IsEasyWorkout().Should().Be(expected);
    }
}

public class TrainingAvailabilityTests
{
    [Fact]
    public void Create_WithValidSchedule_ShouldCreate()
    {
        // Arrange & Act
        var availability = TrainingAvailability.Create(
            WorkoutType.Speed,
            WorkoutType.Recovery,
            WorkoutType.TempoRun,
            WorkoutType.EasyRun,
            WorkoutType.LongRun,
            WorkoutType.Rest,
            WorkoutType.Rest);

        // Assert
        availability.Monday.Should().Be(WorkoutType.Speed);
        availability.Tuesday.Should().Be(WorkoutType.Recovery);
        availability.Wednesday.Should().Be(WorkoutType.TempoRun);
        availability.Thursday.Should().Be(WorkoutType.EasyRun);
        availability.Friday.Should().Be(WorkoutType.LongRun);
        availability.Saturday.Should().Be(WorkoutType.Rest);
        availability.Sunday.Should().Be(WorkoutType.Rest);
    }

    [Fact]
    public void Create_WithConsecutiveQualityWorkouts_ShouldThrow()
    {
        // Arrange & Act
        var act = () => TrainingAvailability.Create(
            WorkoutType.Speed,      // Monday - quality
            WorkoutType.TempoRun,   // Tuesday - quality (consecutive!)
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*consecutive*Monday*Tuesday*");
    }

    [Fact]
    public void Create_WithConsecutiveQualityWorkouts_SundayToMonday_ShouldThrow()
    {
        // Arrange & Act - Sunday wraps to Monday
        var act = () => TrainingAvailability.Create(
            WorkoutType.Speed,      // Monday - quality
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.LongRun);   // Sunday - quality (consecutive with Monday!)

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*consecutive*Sunday*Monday*");
    }

    [Fact]
    public void Create_WithAlternatingQualityAndEasy_ShouldSucceed()
    {
        // Arrange & Act
        var availability = TrainingAvailability.Create(
            WorkoutType.Speed,
            WorkoutType.Recovery,
            WorkoutType.TempoRun,
            WorkoutType.EasyRun,
            WorkoutType.LongRun,
            WorkoutType.Recovery,
            WorkoutType.Rest);

        // Assert
        availability.Should().NotBeNull();
    }

    [Fact]
    public void Default_ShouldReturnAllRest()
    {
        // Arrange & Act
        var availability = TrainingAvailability.Default();

        // Assert
        availability.Monday.Should().Be(WorkoutType.Rest);
        availability.Tuesday.Should().Be(WorkoutType.Rest);
        availability.Wednesday.Should().Be(WorkoutType.Rest);
        availability.Thursday.Should().Be(WorkoutType.Rest);
        availability.Friday.Should().Be(WorkoutType.Rest);
        availability.Saturday.Should().Be(WorkoutType.Rest);
        availability.Sunday.Should().Be(WorkoutType.Rest);
    }

    [Fact]
    public void GetWorkoutForDay_ShouldReturnCorrectWorkout()
    {
        // Arrange
        var availability = TrainingAvailability.Create(
            WorkoutType.Speed,
            WorkoutType.Recovery,
            WorkoutType.TempoRun,
            WorkoutType.EasyRun,
            WorkoutType.LongRun,
            WorkoutType.Recovery,
            WorkoutType.Rest);

        // Act & Assert
        availability.GetWorkoutForDay(DayOfWeek.Monday).Should().Be(WorkoutType.Speed);
        availability.GetWorkoutForDay(DayOfWeek.Wednesday).Should().Be(WorkoutType.TempoRun);
        availability.GetWorkoutForDay(DayOfWeek.Friday).Should().Be(WorkoutType.LongRun);
        availability.GetWorkoutForDay(DayOfWeek.Sunday).Should().Be(WorkoutType.Rest);
    }
}

public class GoalDistanceTests
{
    [Theory]
    [InlineData(DistanceType.Distance1600m, 1.6)]
    [InlineData(DistanceType.Distance5K, 5)]
    [InlineData(DistanceType.Distance10K, 10)]
    [InlineData(DistanceType.Distance16K, 16)]
    [InlineData(DistanceType.HalfMarathon, 21.0975)]
    [InlineData(DistanceType.Marathon, 42.195)]
    public void GetDistanceInKm_ForPredefinedTypes_ShouldReturnCorrectDistance(DistanceType type, decimal expected)
    {
        // Arrange
        var distance = GoalDistance.Create(type);

        // Act & Assert
        distance.GetDistanceInKm().Should().Be(expected);
    }

    [Fact]
    public void Create_WithCustomDistance_ShouldSetCustomDistanceKm()
    {
        // Arrange & Act
        var distance = GoalDistance.Create(DistanceType.Custom, 15.5m);

        // Assert
        distance.DistanceType.Should().Be(DistanceType.Custom);
        distance.CustomDistanceKm.Should().Be(15.5m);
        distance.GetDistanceInKm().Should().Be(15.5m);
    }

    [Fact]
    public void Create_WithCustomType_WithoutDistance_ShouldThrow()
    {
        // Arrange & Act
        var act = () => GoalDistance.Create(DistanceType.Custom, null);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Custom distance*greater than 0*");
    }

    [Fact]
    public void Create_WithCustomType_WithZeroDistance_ShouldThrow()
    {
        // Arrange & Act
        var act = () => GoalDistance.Create(DistanceType.Custom, 0);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Custom distance*greater than 0*");
    }

    [Fact]
    public void Create_WithCustomType_WithNegativeDistance_ShouldThrow()
    {
        // Arrange & Act
        var act = () => GoalDistance.Create(DistanceType.Custom, -5m);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Custom distance*greater than 0*");
    }

    [Fact]
    public void Create_WithNonCustomType_ShouldIgnoreCustomDistanceKm()
    {
        // Arrange & Act
        var distance = GoalDistance.Create(DistanceType.Distance5K, 99m);

        // Assert
        distance.CustomDistanceKm.Should().BeNull();
        distance.GetDistanceInKm().Should().Be(5m);
    }

    [Theory]
    [InlineData(DistanceType.Distance1600m, "1600m")]
    [InlineData(DistanceType.Distance5K, "5K")]
    [InlineData(DistanceType.Distance10K, "10K")]
    [InlineData(DistanceType.Distance16K, "16K")]
    [InlineData(DistanceType.HalfMarathon, "Half Marathon")]
    [InlineData(DistanceType.Marathon, "Marathon")]
    public void GetDisplayName_ForPredefinedTypes_ShouldReturnCorrectName(DistanceType type, string expected)
    {
        // Arrange
        var distance = GoalDistance.Create(type);

        // Act & Assert
        distance.GetDisplayName().Should().Be(expected);
    }

    [Fact]
    public void GetDisplayName_ForCustomType_ShouldReturnDistanceWithKm()
    {
        // Arrange
        var distance = GoalDistance.Create(DistanceType.Custom, 12.5m);

        // Act & Assert
        distance.GetDisplayName().Should().MatchRegex(@"12[.,]5 km");
    }
}
