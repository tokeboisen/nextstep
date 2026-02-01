using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class TrainingAvailability : ValueObject
{
    public WorkoutType Monday { get; private set; }
    public WorkoutType Tuesday { get; private set; }
    public WorkoutType Wednesday { get; private set; }
    public WorkoutType Thursday { get; private set; }
    public WorkoutType Friday { get; private set; }
    public WorkoutType Saturday { get; private set; }
    public WorkoutType Sunday { get; private set; }

    private TrainingAvailability() { }

    private TrainingAvailability(
        WorkoutType monday,
        WorkoutType tuesday,
        WorkoutType wednesday,
        WorkoutType thursday,
        WorkoutType friday,
        WorkoutType saturday,
        WorkoutType sunday)
    {
        Monday = monday;
        Tuesday = tuesday;
        Wednesday = wednesday;
        Thursday = thursday;
        Friday = friday;
        Saturday = saturday;
        Sunday = sunday;
    }

    public static TrainingAvailability Create(
        WorkoutType monday,
        WorkoutType tuesday,
        WorkoutType wednesday,
        WorkoutType thursday,
        WorkoutType friday,
        WorkoutType saturday,
        WorkoutType sunday)
    {
        var availability = new TrainingAvailability(
            monday, tuesday, wednesday, thursday, friday, saturday, sunday);

        var validationError = availability.ValidateNoConsecutiveQualityWorkouts();
        if (validationError != null)
        {
            throw new ArgumentException(validationError);
        }

        return availability;
    }

    public static TrainingAvailability Default()
    {
        return new TrainingAvailability(
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest,
            WorkoutType.Rest);
    }

    public WorkoutType GetWorkoutForDay(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday => Monday,
            DayOfWeek.Tuesday => Tuesday,
            DayOfWeek.Wednesday => Wednesday,
            DayOfWeek.Thursday => Thursday,
            DayOfWeek.Friday => Friday,
            DayOfWeek.Saturday => Saturday,
            DayOfWeek.Sunday => Sunday,
            _ => throw new ArgumentOutOfRangeException(nameof(day))
        };
    }

    public string? ValidateNoConsecutiveQualityWorkouts()
    {
        var days = new[]
        {
            (DayOfWeek.Monday, Monday),
            (DayOfWeek.Tuesday, Tuesday),
            (DayOfWeek.Wednesday, Wednesday),
            (DayOfWeek.Thursday, Thursday),
            (DayOfWeek.Friday, Friday),
            (DayOfWeek.Saturday, Saturday),
            (DayOfWeek.Sunday, Sunday)
        };

        for (int i = 0; i < days.Length; i++)
        {
            var current = days[i];
            var next = days[(i + 1) % days.Length]; // Wraps Sunday -> Monday

            if (current.Item2.IsQualityWorkout() && next.Item2.IsQualityWorkout())
            {
                return $"Cannot have quality workouts on consecutive days: {current.Item1} ({current.Item2}) and {next.Item1} ({next.Item2})";
            }
        }

        return null;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Monday;
        yield return Tuesday;
        yield return Wednesday;
        yield return Thursday;
        yield return Friday;
        yield return Saturday;
        yield return Sunday;
    }
}
