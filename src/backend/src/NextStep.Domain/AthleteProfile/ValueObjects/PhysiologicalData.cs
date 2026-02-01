using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class PhysiologicalData : ValueObject
{
    public int? MaxHeartRate { get; private set; }
    public int? LactateThresholdHeartRate { get; private set; }
    public TimeSpan? LactateThresholdPace { get; private set; }

    private PhysiologicalData() { }

    private PhysiologicalData(int? maxHeartRate, int? lactateThresholdHeartRate, TimeSpan? lactateThresholdPace)
    {
        MaxHeartRate = maxHeartRate;
        LactateThresholdHeartRate = lactateThresholdHeartRate;
        LactateThresholdPace = lactateThresholdPace;
    }

    public static PhysiologicalData Create(int? maxHeartRate, int? lactateThresholdHeartRate, TimeSpan? lactateThresholdPace)
    {
        if (maxHeartRate.HasValue && (maxHeartRate < 100 || maxHeartRate > 250))
            throw new ArgumentException("Max heart rate must be between 100 and 250 bpm", nameof(maxHeartRate));

        if (lactateThresholdHeartRate.HasValue && (lactateThresholdHeartRate < 80 || lactateThresholdHeartRate > 220))
            throw new ArgumentException("Lactate threshold heart rate must be between 80 and 220 bpm", nameof(lactateThresholdHeartRate));

        if (lactateThresholdHeartRate.HasValue && maxHeartRate.HasValue && lactateThresholdHeartRate > maxHeartRate)
            throw new ArgumentException("Lactate threshold heart rate cannot exceed max heart rate", nameof(lactateThresholdHeartRate));

        if (lactateThresholdPace.HasValue)
        {
            var minPace = TimeSpan.FromMinutes(2); // 2:00 min/km
            var maxPace = TimeSpan.FromMinutes(10); // 10:00 min/km
            if (lactateThresholdPace < minPace || lactateThresholdPace > maxPace)
                throw new ArgumentException("Lactate threshold pace must be between 2:00 and 10:00 min/km", nameof(lactateThresholdPace));
        }

        return new PhysiologicalData(maxHeartRate, lactateThresholdHeartRate, lactateThresholdPace);
    }

    public static PhysiologicalData Empty() => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MaxHeartRate;
        yield return LactateThresholdHeartRate;
        yield return LactateThresholdPace;
    }
}
