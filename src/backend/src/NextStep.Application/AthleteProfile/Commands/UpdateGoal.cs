using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdateGoalCommand(
    Guid GoalId,
    DateOnly RaceDate,
    TimeSpan TargetTime,
    DistanceType DistanceType,
    decimal? CustomDistanceKm
) : IRequest;

public class UpdateGoalCommandHandler : IRequestHandler<UpdateGoalCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdateGoalCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        var distance = GoalDistance.Create(request.DistanceType, request.CustomDistanceKm);
        athlete.UpdateGoal(request.GoalId, request.RaceDate, request.TargetTime, distance);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
