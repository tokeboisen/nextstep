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
            .Include(a => a.Goals)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Athlete?> GetSingleAthleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Athletes
            .Include(a => a.Goals)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Athlete athlete, CancellationToken cancellationToken = default)
    {
        await _context.Athletes.AddAsync(athlete, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Athlete athlete, CancellationToken cancellationToken = default)
    {
        // Get existing goal IDs from database
        var existingGoalIds = await _context.Goals
            .Where(g => g.AthleteId == athlete.Id)
            .Select(g => g.Id)
            .ToListAsync(cancellationToken);

        // Detect new goals that need to be added
        foreach (var goal in athlete.Goals)
        {
            if (!existingGoalIds.Contains(goal.Id))
            {
                // New goal - mark as Added explicitly
                _context.Entry(goal).State = EntityState.Added;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
