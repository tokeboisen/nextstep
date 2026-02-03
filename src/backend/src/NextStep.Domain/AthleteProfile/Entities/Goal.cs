using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Domain.AthleteProfile.Entities;

public class Goal
{
    public Guid Id { get; private set; }
    public DateOnly RaceDate { get; private set; }
    public TimeSpan TargetTime { get; private set; }
    public GoalDistance Distance { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    // Foreign key
    public Guid AthleteId { get; private set; }

    private Goal() { }

    public static Goal Create(
        DateOnly raceDate,
        TimeSpan targetTime,
        GoalDistance distance,
        bool isPrimary = false)
    {
        return new Goal
        {
            Id = Guid.NewGuid(),
            RaceDate = raceDate,
            TargetTime = targetTime,
            Distance = distance,
            IsPrimary = isPrimary
        };
    }

    public void Update(DateOnly raceDate, TimeSpan targetTime, GoalDistance distance)
    {
        RaceDate = raceDate;
        TargetTime = targetTime;
        Distance = distance;
    }

    internal void SetPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
    }
}
