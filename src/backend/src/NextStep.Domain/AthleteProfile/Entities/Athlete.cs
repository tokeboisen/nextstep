using NextStep.Domain.AthleteProfile.ValueObjects;
using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.Entities;

public class Athlete : Entity<Guid>
{
    public PersonalInfo PersonalInfo { get; private set; } = null!;
    public PhysiologicalData PhysiologicalData { get; private set; } = null!;
    public TrainingAccess TrainingAccess { get; private set; } = null!;
    public TrainingAvailability TrainingAvailability { get; private set; } = null!;

    private readonly List<Goal> _goals = new();
    public IReadOnlyList<Goal> Goals => _goals.AsReadOnly();

    public IReadOnlyList<HeartRateZone> HeartRateZones => CalculateHeartRateZones();
    public IReadOnlyList<PaceZone> PaceZones => CalculatePaceZones();

    private Athlete() : base() { }

    private Athlete(Guid id, PersonalInfo personalInfo) : base(id)
    {
        PersonalInfo = personalInfo;
        PhysiologicalData = PhysiologicalData.Empty();
        TrainingAccess = TrainingAccess.Default();
        TrainingAvailability = TrainingAvailability.Default();
    }

    public static Athlete Create(string name, DateOnly birthDate)
    {
        var personalInfo = PersonalInfo.Create(name, birthDate);
        return new Athlete(Guid.NewGuid(), personalInfo);
    }

    public static Athlete CreateWithId(Guid id, string name, DateOnly birthDate)
    {
        var personalInfo = PersonalInfo.Create(name, birthDate);
        return new Athlete(id, personalInfo);
    }

    public void UpdatePersonalInfo(string name, DateOnly birthDate)
    {
        PersonalInfo = PersonalInfo.Create(name, birthDate);
    }

    public void UpdatePhysiologicalData(int? maxHeartRate, int? lactateThresholdHeartRate, TimeSpan? lactateThresholdPace)
    {
        PhysiologicalData = PhysiologicalData.Create(maxHeartRate, lactateThresholdHeartRate, lactateThresholdPace);
    }

    public void UpdateTrainingAccess(bool hasTrackAccess)
    {
        TrainingAccess = TrainingAccess.Create(hasTrackAccess);
    }

    public void UpdateTrainingAvailability(
        WorkoutType monday,
        WorkoutType tuesday,
        WorkoutType wednesday,
        WorkoutType thursday,
        WorkoutType friday,
        WorkoutType saturday,
        WorkoutType sunday)
    {
        TrainingAvailability = TrainingAvailability.Create(
            monday, tuesday, wednesday, thursday, friday, saturday, sunday);
    }

    public Goal AddGoal(DateOnly raceDate, TimeSpan targetTime, GoalDistance distance)
    {
        var isPrimary = _goals.Count == 0;
        var goal = Goal.Create(raceDate, targetTime, distance, isPrimary);
        _goals.Add(goal);
        return goal;
    }

    public void UpdateGoal(Guid goalId, DateOnly raceDate, TimeSpan targetTime, GoalDistance distance)
    {
        var goal = _goals.FirstOrDefault(g => g.Id == goalId)
            ?? throw new InvalidOperationException($"Goal with id {goalId} not found");

        goal.Update(raceDate, targetTime, distance);
    }

    public void DeleteGoal(Guid goalId)
    {
        var goal = _goals.FirstOrDefault(g => g.Id == goalId)
            ?? throw new InvalidOperationException($"Goal with id {goalId} not found");

        if (goal.IsPrimary && _goals.Count > 1)
        {
            throw new InvalidOperationException("Cannot delete primary goal when other goals exist. Set another goal as primary first.");
        }

        _goals.Remove(goal);
    }

    public void SetPrimaryGoal(Guid goalId)
    {
        var goal = _goals.FirstOrDefault(g => g.Id == goalId)
            ?? throw new InvalidOperationException($"Goal with id {goalId} not found");

        foreach (var g in _goals)
        {
            g.SetPrimary(g.Id == goalId);
        }
    }

    private IReadOnlyList<HeartRateZone> CalculateHeartRateZones()
    {
        if (!PhysiologicalData.LactateThresholdHeartRate.HasValue)
            return Array.Empty<HeartRateZone>();

        return HeartRateZone.CalculateFromLactateThreshold(PhysiologicalData.LactateThresholdHeartRate.Value);
    }

    private IReadOnlyList<PaceZone> CalculatePaceZones()
    {
        if (!PhysiologicalData.LactateThresholdPace.HasValue)
            return Array.Empty<PaceZone>();

        return PaceZone.CalculateFromLactateThreshold(PhysiologicalData.LactateThresholdPace.Value);
    }
}
