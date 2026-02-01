using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdateHeartRateZonesCommand(
    List<HeartRateZoneInput> Zones
) : IRequest;

public record HeartRateZoneInput(
    int ZoneNumber,
    string Name,
    int MinBpm,
    int MaxBpm
);

public class UpdateHeartRateZonesCommandHandler : IRequestHandler<UpdateHeartRateZonesCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdateHeartRateZonesCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdateHeartRateZonesCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        var zones = request.Zones
            .Select(z => HeartRateZone.Create(z.ZoneNumber, z.Name, z.MinBpm, z.MaxBpm))
            .ToList();

        athlete.SetHeartRateZones(zones);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
