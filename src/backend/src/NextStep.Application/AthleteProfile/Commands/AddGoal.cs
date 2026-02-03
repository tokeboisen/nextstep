using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Application.AthleteProfile.Commands;

public record AddGoalCommand(
    DateOnly RaceDate,
    TimeSpan TargetTime,
    DistanceType DistanceType,
    decimal? CustomDistanceKm
) : IRequest<Guid>;

public class AddGoalCommandHandler : IRequestHandler<AddGoalCommand, Guid>
{
    private readonly IAthleteRepository _athleteRepository;

    public AddGoalCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<Guid> Handle(AddGoalCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        var distance = GoalDistance.Create(request.DistanceType, request.CustomDistanceKm);
        var goal = athlete.AddGoal(request.RaceDate, request.TargetTime, distance);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);

        return goal.Id;
    }
}
