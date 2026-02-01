using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdateTrainingAccessCommand(
    bool HasTrackAccess
) : IRequest;

public class UpdateTrainingAccessCommandHandler : IRequestHandler<UpdateTrainingAccessCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdateTrainingAccessCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdateTrainingAccessCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        athlete.UpdateTrainingAccess(request.HasTrackAccess);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
