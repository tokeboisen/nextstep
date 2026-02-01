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

    public static PaceZone Create(int zoneNumber, string name, TimeSpan minPacePerKm, TimeSpan maxPacePerKm)
    {
        if (zoneNumber < 1 || zoneNumber > 6)
            throw new ArgumentException("Zone number must be between 1 and 6", nameof(zoneNumber));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Zone name cannot be empty", nameof(name));

        var minSeconds = 120; // 2:00 min/km
        var maxSeconds = 900; // 15:00 min/km

        if (minPacePerKm.TotalSeconds < minSeconds || minPacePerKm.TotalSeconds > maxSeconds)
            throw new ArgumentException("Min pace must be between 2:00 and 15:00 min/km", nameof(minPacePerKm));

        if (maxPacePerKm.TotalSeconds < minSeconds || maxPacePerKm.TotalSeconds > maxSeconds)
            throw new ArgumentException("Max pace must be between 2:00 and 15:00 min/km", nameof(maxPacePerKm));

        // Note: For pace, min pace is FASTER (lower time), max pace is SLOWER (higher time)
        if (minPacePerKm >= maxPacePerKm)
            throw new ArgumentException("Min pace (faster) must be less than Max pace (slower)", nameof(minPacePerKm));

        return new PaceZone(zoneNumber, name.Trim(), minPacePerKm, maxPacePerKm);
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
