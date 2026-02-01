namespace NextStep.Domain.AthleteProfile.ValueObjects;

public enum WorkoutType
{
    Rest = 0,
    CrossHIIT = 1,
    Recovery = 2,
    EasyRun = 3,
    Speed = 4,
    TempoRun = 5,
    LongRun = 6
}

public static class WorkoutTypeExtensions
{
    private static readonly HashSet<WorkoutType> QualityWorkouts = new()
    {
        WorkoutType.CrossHIIT,
        WorkoutType.Speed,
        WorkoutType.TempoRun,
        WorkoutType.LongRun
    };

    public static bool IsQualityWorkout(this WorkoutType workoutType)
    {
        return QualityWorkouts.Contains(workoutType);
    }

    public static bool IsEasyWorkout(this WorkoutType workoutType)
    {
        return !workoutType.IsQualityWorkout();
    }
}
