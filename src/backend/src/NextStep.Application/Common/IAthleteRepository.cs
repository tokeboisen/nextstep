using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Application.Common;

public interface IAthleteRepository
{
    Task<Athlete?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Athlete?> GetSingleAthleteAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Athlete athlete, CancellationToken cancellationToken = default);
    Task UpdateAsync(Athlete athlete, CancellationToken cancellationToken = default);
}
