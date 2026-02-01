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

    public static HeartRateZone Create(int zoneNumber, string name, int minBpm, int maxBpm)
    {
        if (zoneNumber < 1 || zoneNumber > 6)
            throw new ArgumentException("Zone number must be between 1 and 6", nameof(zoneNumber));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Zone name cannot be empty", nameof(name));

        if (minBpm < 50 || minBpm > 250)
            throw new ArgumentException("Min BPM must be between 50 and 250", nameof(minBpm));

        if (maxBpm < 50 || maxBpm > 250)
            throw new ArgumentException("Max BPM must be between 50 and 250", nameof(maxBpm));

        if (minBpm >= maxBpm)
            throw new ArgumentException("Min BPM must be less than Max BPM", nameof(minBpm));

        return new HeartRateZone(zoneNumber, name.Trim(), minBpm, maxBpm);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ZoneNumber;
        yield return Name;
        yield return MinBpm;
        yield return MaxBpm;
    }
}
