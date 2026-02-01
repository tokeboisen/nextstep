using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class TrainingAccess : ValueObject
{
    public bool HasTrackAccess { get; private set; }

    private TrainingAccess() { }

    private TrainingAccess(bool hasTrackAccess)
    {
        HasTrackAccess = hasTrackAccess;
    }

    public static TrainingAccess Create(bool hasTrackAccess)
    {
        return new TrainingAccess(hasTrackAccess);
    }

    public static TrainingAccess Default() => new(false);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return HasTrackAccess;
    }
}
