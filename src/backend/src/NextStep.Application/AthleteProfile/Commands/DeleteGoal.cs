using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record DeleteGoalCommand(Guid GoalId) : IRequest;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public DeleteGoalCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        athlete.DeleteGoal(request.GoalId);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
