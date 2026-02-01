using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdatePaceZonesCommand(
    List<PaceZoneInput> Zones
) : IRequest;

public record PaceZoneInput(
    int ZoneNumber,
    string Name,
    string MinPace,
    string MaxPace
);

public class UpdatePaceZonesCommandHandler : IRequestHandler<UpdatePaceZonesCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdatePaceZonesCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdatePaceZonesCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        var zones = request.Zones
            .Select(z => PaceZone.Create(
                z.ZoneNumber,
                z.Name,
                ParsePace(z.MinPace),
                ParsePace(z.MaxPace)))
            .ToList();

        athlete.SetPaceZones(zones);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }

    private static TimeSpan ParsePace(string pace)
    {
        var parts = pace.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException($"Invalid pace format: {pace}. Expected format: mm:ss");

        if (!int.TryParse(parts[0], out var minutes) || !int.TryParse(parts[1], out var seconds))
            throw new ArgumentException($"Invalid pace format: {pace}. Expected format: mm:ss");

        return new TimeSpan(0, minutes, seconds);
    }
}
