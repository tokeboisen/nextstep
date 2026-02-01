using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdatePhysiologicalDataCommand(
    int? MaxHeartRate,
    int? LactateThreshold
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

        athlete.UpdatePhysiologicalData(request.MaxHeartRate, request.LactateThreshold);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
