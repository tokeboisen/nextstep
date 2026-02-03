using MediatR;
using NextStep.Application.AthleteProfile.DTOs;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Queries;

public record GetAthleteProfileQuery : IRequest<AthleteDto?>;

public class GetAthleteProfileQueryHandler : IRequestHandler<GetAthleteProfileQuery, AthleteDto?>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAthleteProfileQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<AthleteDto?> Handle(GetAthleteProfileQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken);

        if (athlete is null)
            return null;

        return new AthleteDto(
            athlete.Id,
            new PersonalInfoDto(
                athlete.PersonalInfo.Name,
                athlete.PersonalInfo.BirthDate,
                athlete.PersonalInfo.CalculateAge()
            ),
            new PhysiologicalDataDto(
                athlete.PhysiologicalData.MaxHeartRate,
                athlete.PhysiologicalData.LactateThresholdHeartRate,
                FormatPace(athlete.PhysiologicalData.LactateThresholdPace)
            ),
            new TrainingAccessDto(
                athlete.TrainingAccess.HasTrackAccess
            ),
            new TrainingAvailabilityDto(
                athlete.TrainingAvailability.Monday.ToString(),
                athlete.TrainingAvailability.Tuesday.ToString(),
                athlete.TrainingAvailability.Wednesday.ToString(),
                athlete.TrainingAvailability.Thursday.ToString(),
                athlete.TrainingAvailability.Friday.ToString(),
                athlete.TrainingAvailability.Saturday.ToString(),
                athlete.TrainingAvailability.Sunday.ToString()
            ),
            athlete.HeartRateZones.Select(z => new HeartRateZoneDto(
                z.ZoneNumber,
                z.Name,
                z.MinBpm,
                z.MaxBpm
            )).ToList(),
            athlete.PaceZones.Select(z => new PaceZoneDto(
                z.ZoneNumber,
                z.Name,
                z.FormatMinPace(),
                z.FormatMaxPace()
            )).ToList(),
            athlete.Goals.Select(g => new GoalDto(
                g.Id,
                g.RaceDate,
                FormatTargetTime(g.TargetTime),
                g.Distance.DistanceType.ToString(),
                g.Distance.CustomDistanceKm,
                g.Distance.GetDisplayName(),
                g.IsPrimary
            )).ToList()
        );
    }

    private static string? FormatPace(TimeSpan? pace)
    {
        if (!pace.HasValue)
            return null;

        return $"{(int)pace.Value.TotalMinutes}:{pace.Value.Seconds:D2}";
    }

    private static string FormatTargetTime(TimeSpan time)
    {
        if (time.TotalHours >= 1)
        {
            return $"{(int)time.TotalHours}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        return $"{time.Minutes}:{time.Seconds:D2}";
    }
}
