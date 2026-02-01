using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class HeartRateZone : ValueObject
{
    public int ZoneNumber { get; private set; }
    public string Name { get; private set; }
    public int MinBpm { get; private set; }
    public int MaxBpm { get; private set; }

    private HeartRateZone()
    {
        Name = string.Empty;
    }

    private HeartRateZone(int zoneNumber, string name, int minBpm, int maxBpm)
    {
        ZoneNumber = zoneNumber;
        Name = name;
        MinBpm = minBpm;
        MaxBpm = maxBpm;
    }

    public static IReadOnlyList<HeartRateZone> CalculateFromLactateThreshold(int lactateThresholdHeartRate)
    {
        if (lactateThresholdHeartRate < 80 || lactateThresholdHeartRate > 220)
            throw new ArgumentException("Lactate threshold heart rate must be between 80 and 220 bpm", nameof(lactateThresholdHeartRate));

        var lthr = lactateThresholdHeartRate;

        return new List<HeartRateZone>
        {
            // Zone 1 (Recovery): < 81% of LTHR
            new(1, "Recovery", (int)(lthr * 0.50), (int)(lthr * 0.80)),
            // Zone 2 (Aerobic): 81-89% of LTHR
            new(2, "Aerobic", (int)(lthr * 0.81), (int)(lthr * 0.89)),
            // Zone 3 (Tempo): 90-95% of LTHR
            new(3, "Tempo", (int)(lthr * 0.90), (int)(lthr * 0.95)),
            // Zone 4 (Threshold): 96-100% of LTHR
            new(4, "Threshold", (int)(lthr * 0.96), lthr),
            // Zone 5 (VO2max): > 100% of LTHR
            new(5, "VO2max", lthr + 1, (int)(lthr * 1.15))
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ZoneNumber;
        yield return Name;
        yield return MinBpm;
        yield return MaxBpm;
    }
}
