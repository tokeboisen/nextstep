using NextStep.Domain.AthleteProfile.ValueObjects;
using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.Entities;

public class Athlete : Entity<Guid>
{
    public PersonalInfo PersonalInfo { get; private set; } = null!;
    public PhysiologicalData PhysiologicalData { get; private set; } = null!;
    public TrainingAccess TrainingAccess { get; private set; } = null!;

    private readonly List<HeartRateZone> _heartRateZones = new();
    public IReadOnlyList<HeartRateZone> HeartRateZones => _heartRateZones.AsReadOnly();

    private readonly List<PaceZone> _paceZones = new();
    public IReadOnlyList<PaceZone> PaceZones => _paceZones.AsReadOnly();

    private Athlete() : base() { }

    private Athlete(Guid id, PersonalInfo personalInfo) : base(id)
    {
        PersonalInfo = personalInfo;
        PhysiologicalData = PhysiologicalData.Empty();
        TrainingAccess = TrainingAccess.Default();
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

    public void UpdatePhysiologicalData(int? maxHeartRate, int? lactateThreshold)
    {
        PhysiologicalData = PhysiologicalData.Create(maxHeartRate, lactateThreshold);
    }

    public void UpdateTrainingAccess(bool hasTrackAccess)
    {
        TrainingAccess = TrainingAccess.Create(hasTrackAccess);
    }

    public void SetHeartRateZones(IEnumerable<HeartRateZone> zones)
    {
        var zoneList = zones.ToList();

        ValidateZones(zoneList.Select(z => z.ZoneNumber).ToList());

        _heartRateZones.Clear();
        _heartRateZones.AddRange(zoneList.OrderBy(z => z.ZoneNumber));
    }

    public void SetPaceZones(IEnumerable<PaceZone> zones)
    {
        var zoneList = zones.ToList();

        ValidateZones(zoneList.Select(z => z.ZoneNumber).ToList());

        _paceZones.Clear();
        _paceZones.AddRange(zoneList.OrderBy(z => z.ZoneNumber));
    }

    public void ClearHeartRateZones()
    {
        _heartRateZones.Clear();
    }

    public void ClearPaceZones()
    {
        _paceZones.Clear();
    }

    private static void ValidateZones(List<int> zoneNumbers)
    {
        if (zoneNumbers.Count != zoneNumbers.Distinct().Count())
            throw new ArgumentException("Duplicate zone numbers are not allowed");
    }
}
