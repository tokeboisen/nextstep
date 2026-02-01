using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.ValueObjects;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdateTrainingAvailabilityCommand(
    WorkoutType Monday,
    WorkoutType Tuesday,
    WorkoutType Wednesday,
    WorkoutType Thursday,
    WorkoutType Friday,
    WorkoutType Saturday,
    WorkoutType Sunday
) : IRequest;

public class UpdateTrainingAvailabilityCommandHandler : IRequestHandler<UpdateTrainingAvailabilityCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdateTrainingAvailabilityCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdateTrainingAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        athlete.UpdateTrainingAvailability(
            request.Monday,
            request.Tuesday,
            request.Wednesday,
            request.Thursday,
            request.Friday,
            request.Saturday,
            request.Sunday);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
