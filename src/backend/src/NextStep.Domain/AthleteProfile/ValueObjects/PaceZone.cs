using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class PaceZone : ValueObject
{
    public int ZoneNumber { get; private set; }
    public string Name { get; private set; }
    public TimeSpan MinPacePerKm { get; private set; }
    public TimeSpan MaxPacePerKm { get; private set; }

    private PaceZone()
    {
        Name = string.Empty;
    }

    private PaceZone(int zoneNumber, string name, TimeSpan minPacePerKm, TimeSpan maxPacePerKm)
    {
        ZoneNumber = zoneNumber;
        Name = name;
        MinPacePerKm = minPacePerKm;
        MaxPacePerKm = maxPacePerKm;
    }

    public static IReadOnlyList<PaceZone> CalculateFromLactateThreshold(TimeSpan lactateThresholdPace)
    {
        var minPace = TimeSpan.FromMinutes(2);
        var maxPace = TimeSpan.FromMinutes(10);
        if (lactateThresholdPace < minPace || lactateThresholdPace > maxPace)
            throw new ArgumentException("Lactate threshold pace must be between 2:00 and 10:00 min/km", nameof(lactateThresholdPace));

        var ltpSeconds = lactateThresholdPace.TotalSeconds;

        // Note: For pace, faster = lower time, slower = higher time
        // Zone percentages are relative to LTP - higher % means slower pace
        return new List<PaceZone>
        {
            // Zone 1 (Recovery): > 129% of LTP (slowest)
            new(1, "Recovery", TimeSpan.FromSeconds(ltpSeconds * 1.29), TimeSpan.FromSeconds(ltpSeconds * 1.50)),
            // Zone 2 (Aerobic): 114-129% of LTP
            new(2, "Aerobic", TimeSpan.FromSeconds(ltpSeconds * 1.14), TimeSpan.FromSeconds(ltpSeconds * 1.29)),
            // Zone 3 (Tempo): 106-113% of LTP
            new(3, "Tempo", TimeSpan.FromSeconds(ltpSeconds * 1.06), TimeSpan.FromSeconds(ltpSeconds * 1.13)),
            // Zone 4 (Threshold): 99-105% of LTP
            new(4, "Threshold", TimeSpan.FromSeconds(ltpSeconds * 0.99), TimeSpan.FromSeconds(ltpSeconds * 1.05)),
            // Zone 5 (VO2max): < 99% of LTP (fastest)
            new(5, "VO2max", TimeSpan.FromSeconds(ltpSeconds * 0.85), TimeSpan.FromSeconds(ltpSeconds * 0.99))
        };
    }

    public string FormatMinPace() => FormatPace(MinPacePerKm);
    public string FormatMaxPace() => FormatPace(MaxPacePerKm);

    private static string FormatPace(TimeSpan pace)
    {
        return $"{(int)pace.TotalMinutes}:{pace.Seconds:D2}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ZoneNumber;
        yield return Name;
        yield return MinPacePerKm;
        yield return MaxPacePerKm;
    }
}
