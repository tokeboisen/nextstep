using MediatR;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Application.AthleteProfile.Commands;

public record CreateAthleteCommand(
    string Name,
    DateOnly BirthDate
) : IRequest<Guid>;

public class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, Guid>
{
    private readonly IAthleteRepository _athleteRepository;

    public CreateAthleteCommandHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<Guid> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var existingAthlete = await _athleteRepository.GetSingleAthleteAsync(cancellationToken);
        if (existingAthlete is not null)
            throw new InvalidOperationException("An athlete profile already exists. Only one athlete is allowed.");

        var athlete = Athlete.Create(request.Name, request.BirthDate);

        await _athleteRepository.AddAsync(athlete, cancellationToken);

        return athlete.Id;
    }
}
