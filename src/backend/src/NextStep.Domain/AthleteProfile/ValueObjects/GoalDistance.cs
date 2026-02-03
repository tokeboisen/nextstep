namespace NextStep.Domain.AthleteProfile.ValueObjects;

public enum DistanceType
{
    Distance1600m = 0,
    Distance5K = 1,
    Distance10K = 2,
    Distance16K = 3,
    HalfMarathon = 4,
    Marathon = 5,
    Custom = 6
}

public record GoalDistance
{
    public DistanceType DistanceType { get; init; }
    public decimal? CustomDistanceKm { get; init; }

    private GoalDistance() { }

    public static GoalDistance Create(DistanceType distanceType, decimal? customDistanceKm = null)
    {
        if (distanceType == DistanceType.Custom)
        {
            if (!customDistanceKm.HasValue || customDistanceKm.Value <= 0)
            {
                throw new ArgumentException("Custom distance must be greater than 0 when DistanceType is Custom");
            }
        }

        return new GoalDistance
        {
            DistanceType = distanceType,
            CustomDistanceKm = distanceType == DistanceType.Custom ? customDistanceKm : null
        };
    }

    public decimal GetDistanceInKm()
    {
        return DistanceType switch
        {
            DistanceType.Distance1600m => 1.6m,
            DistanceType.Distance5K => 5m,
            DistanceType.Distance10K => 10m,
            DistanceType.Distance16K => 16m,
            DistanceType.HalfMarathon => 21.0975m,
            DistanceType.Marathon => 42.195m,
            DistanceType.Custom => CustomDistanceKm ?? throw new InvalidOperationException("Custom distance not set"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public string GetDisplayName()
    {
        return DistanceType switch
        {
            DistanceType.Distance1600m => "1600m",
            DistanceType.Distance5K => "5K",
            DistanceType.Distance10K => "10K",
            DistanceType.Distance16K => "16K",
            DistanceType.HalfMarathon => "Half Marathon",
            DistanceType.Marathon => "Marathon",
            DistanceType.Custom => $"{CustomDistanceKm:0.##} km",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
