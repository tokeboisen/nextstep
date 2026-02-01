using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdatePhysiologicalDataCommand(
    int? MaxHeartRate,
    int? LactateThresholdHeartRate,
    string? LactateThresholdPace
) : IRequest;

public class UpdatePhysiologicalDataCommandHandler : IRequestHandler<UpdatePhysiologicalDataCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdatePhysiologicalDataCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdatePhysiologicalDataCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        var lactateThresholdPace = ParsePace(request.LactateThresholdPace);

        athlete.UpdatePhysiologicalData(request.MaxHeartRate, request.LactateThresholdHeartRate, lactateThresholdPace);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }

    private static TimeSpan? ParsePace(string? pace)
    {
        if (string.IsNullOrWhiteSpace(pace))
            return null;

        var parts = pace.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException($"Invalid pace format: {pace}. Expected format: mm:ss");

        if (!int.TryParse(parts[0], out var minutes) || !int.TryParse(parts[1], out var seconds))
            throw new ArgumentException($"Invalid pace format: {pace}. Expected format: mm:ss");

        return new TimeSpan(0, minutes, seconds);
    }
}
