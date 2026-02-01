using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class PhysiologicalData : ValueObject
{
    public int? MaxHeartRate { get; private set; }
    public int? LactateThreshold { get; private set; }

    private PhysiologicalData() { }

    private PhysiologicalData(int? maxHeartRate, int? lactateThreshold)
    {
        MaxHeartRate = maxHeartRate;
        LactateThreshold = lactateThreshold;
    }

    public static PhysiologicalData Create(int? maxHeartRate, int? lactateThreshold)
    {
        if (maxHeartRate.HasValue && (maxHeartRate < 100 || maxHeartRate > 250))
            throw new ArgumentException("Max heart rate must be between 100 and 250 bpm", nameof(maxHeartRate));

        if (lactateThreshold.HasValue && (lactateThreshold < 80 || lactateThreshold > 220))
            throw new ArgumentException("Lactate threshold must be between 80 and 220 bpm", nameof(lactateThreshold));

        if (lactateThreshold.HasValue && maxHeartRate.HasValue && lactateThreshold > maxHeartRate)
            throw new ArgumentException("Lactate threshold cannot exceed max heart rate", nameof(lactateThreshold));

        return new PhysiologicalData(maxHeartRate, lactateThreshold);
    }

    public static PhysiologicalData Empty() => new();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MaxHeartRate;
        yield return LactateThreshold;
    }
}
