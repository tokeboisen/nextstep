using MediatR;
using NextStep.Application.Common;

namespace NextStep.Application.AthleteProfile.Commands;

public record UpdatePersonalInfoCommand(
    string Name,
    DateOnly BirthDate
) : IRequest;

public class UpdatePersonalInfoCommandHandler : IRequestHandler<UpdatePersonalInfoCommand>
{
    private readonly IAthleteRepository _athleteRepository;

    public UpdatePersonalInfoCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task Handle(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken)
            ?? throw new InvalidOperationException("Athlete profile not found");

        athlete.UpdatePersonalInfo(request.Name, request.BirthDate);

        await _athleteRepository.UpdateAsync(athlete, cancellationToken);
    }
}
