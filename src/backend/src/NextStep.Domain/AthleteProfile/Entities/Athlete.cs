using NextStep.Domain.AthleteProfile.ValueObjects;
using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.Entities;

public class Athlete : Entity<Guid>
{
    public PersonalInfo PersonalInfo { get; private set; } = null!;
    public PhysiologicalData PhysiologicalData { get; private set; } = null!;
    public TrainingAccess TrainingAccess { get; private set; } = null!;
    public TrainingAvailability TrainingAvailability { get; private set; } = null!;

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
