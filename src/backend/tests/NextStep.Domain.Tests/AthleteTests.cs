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

    [Fact]
    public void AddGoal_FirstGoal_ShouldBePrimary()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance = GoalDistance.Create(DistanceType.Marathon);

        // Act
        var goal = athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance);

        // Assert
        athlete.Goals.Should().HaveCount(1);
        goal.IsPrimary.Should().BeTrue();
    }

    [Fact]
    public void AddGoal_SecondGoal_ShouldNotBePrimary()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance1 = GoalDistance.Create(DistanceType.Marathon);
        var distance2 = GoalDistance.Create(DistanceType.HalfMarathon);
        athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance1);

        // Act
        var goal2 = athlete.AddGoal(new DateOnly(2026, 6, 1), TimeSpan.FromMinutes(100), distance2);

        // Assert
        athlete.Goals.Should().HaveCount(2);
        goal2.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void UpdateGoal_ShouldUpdateGoalData()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance = GoalDistance.Create(DistanceType.Marathon);
        var goal = athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance);
        var newDistance = GoalDistance.Create(DistanceType.HalfMarathon);

        // Act
        athlete.UpdateGoal(goal.Id, new DateOnly(2026, 11, 1), TimeSpan.FromMinutes(95), newDistance);

        // Assert
        var updatedGoal = athlete.Goals.First(g => g.Id == goal.Id);
        updatedGoal.RaceDate.Should().Be(new DateOnly(2026, 11, 1));
        updatedGoal.TargetTime.Should().Be(TimeSpan.FromMinutes(95));
        updatedGoal.Distance.DistanceType.Should().Be(DistanceType.HalfMarathon);
    }

    [Fact]
    public void UpdateGoal_WithInvalidId_ShouldThrow()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance = GoalDistance.Create(DistanceType.Marathon);

        // Act
        var act = () => athlete.UpdateGoal(Guid.NewGuid(), new DateOnly(2026, 10, 15), TimeSpan.FromHours(3), distance);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*not found*");
    }

    [Fact]
    public void DeleteGoal_SingleGoal_ShouldRemoveGoal()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance = GoalDistance.Create(DistanceType.Marathon);
        var goal = athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance);

        // Act
        athlete.DeleteGoal(goal.Id);

        // Assert
        athlete.Goals.Should().BeEmpty();
    }

    [Fact]
    public void DeleteGoal_NonPrimaryGoal_ShouldRemoveGoal()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance1 = GoalDistance.Create(DistanceType.Marathon);
        var distance2 = GoalDistance.Create(DistanceType.HalfMarathon);
        athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance1);
        var goal2 = athlete.AddGoal(new DateOnly(2026, 6, 1), TimeSpan.FromMinutes(100), distance2);

        // Act
        athlete.DeleteGoal(goal2.Id);

        // Assert
        athlete.Goals.Should().HaveCount(1);
    }

    [Fact]
    public void DeleteGoal_PrimaryGoalWithOtherGoals_ShouldThrow()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance1 = GoalDistance.Create(DistanceType.Marathon);
        var distance2 = GoalDistance.Create(DistanceType.HalfMarathon);
        var primaryGoal = athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance1);
        athlete.AddGoal(new DateOnly(2026, 6, 1), TimeSpan.FromMinutes(100), distance2);

        // Act
        var act = () => athlete.DeleteGoal(primaryGoal.Id);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*primary*another goal*");
    }

    [Fact]
    public void DeleteGoal_WithInvalidId_ShouldThrow()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        var act = () => athlete.DeleteGoal(Guid.NewGuid());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*not found*");
    }

    [Fact]
    public void SetPrimaryGoal_ShouldChangePrimaryStatus()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance1 = GoalDistance.Create(DistanceType.Marathon);
        var distance2 = GoalDistance.Create(DistanceType.HalfMarathon);
        var goal1 = athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance1);
        var goal2 = athlete.AddGoal(new DateOnly(2026, 6, 1), TimeSpan.FromMinutes(100), distance2);

        // Act
        athlete.SetPrimaryGoal(goal2.Id);

        // Assert
        athlete.Goals.First(g => g.Id == goal1.Id).IsPrimary.Should().BeFalse();
        athlete.Goals.First(g => g.Id == goal2.Id).IsPrimary.Should().BeTrue();
    }

    [Fact]
    public void SetPrimaryGoal_WithInvalidId_ShouldThrow()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));

        // Act
        var act = () => athlete.SetPrimaryGoal(Guid.NewGuid());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("*not found*");
    }

    [Fact]
    public void Goals_ShouldOnlyHaveOnePrimaryAtATime()
    {
        // Arrange
        var athlete = Athlete.Create("John Doe", new DateOnly(1990, 5, 15));
        var distance1 = GoalDistance.Create(DistanceType.Marathon);
        var distance2 = GoalDistance.Create(DistanceType.HalfMarathon);
        var distance3 = GoalDistance.Create(DistanceType.Distance10K);
        athlete.AddGoal(new DateOnly(2026, 10, 15), TimeSpan.FromHours(3.5), distance1);
        athlete.AddGoal(new DateOnly(2026, 6, 1), TimeSpan.FromMinutes(100), distance2);
        var goal3 = athlete.AddGoal(new DateOnly(2026, 4, 1), TimeSpan.FromMinutes(45), distance3);

        // Act
        athlete.SetPrimaryGoal(goal3.Id);

        // Assert
        athlete.Goals.Count(g => g.IsPrimary).Should().Be(1);
        athlete.Goals.Single(g => g.IsPrimary).Id.Should().Be(goal3.Id);
    }
}
