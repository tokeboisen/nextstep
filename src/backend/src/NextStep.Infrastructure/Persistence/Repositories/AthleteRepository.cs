using Microsoft.EntityFrameworkCore;
using NextStep.Application.Common;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Infrastructure.Persistence.Repositories;

public class AthleteRepository : IAthleteRepository
{
    private readonly NextStepDbContext _context;

    public AthleteRepository(NextStepDbContext context)
    {
        _context = context;
    }

    public async Task<Athlete?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Athletes
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Athlete?> GetSingleAthleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Athletes
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Athlete athlete, CancellationToken cancellationToken = default)
    {
        await _context.Athletes.AddAsync(athlete, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Athlete athlete, CancellationToken cancellationToken = default)
    {
        _context.Athletes.Update(athlete);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
