using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record SetPrimaryGoalCommand(Guid GoalId) : IRequest;

public class SetPrimaryGoalCommandHandler : IRequestHandler<SetPrimaryGoalCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public SetPrimaryGoalCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(SetPrimaryGoalCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        athlete.SetPrimaryGoal(request.GoalId);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
