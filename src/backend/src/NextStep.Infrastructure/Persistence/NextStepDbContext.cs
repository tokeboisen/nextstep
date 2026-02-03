using Microsoft.EntityFrameworkCore;
using NextStep.Domain.AthleteProfile.Entities;

namespace NextStep.Infrastructure.Persistence;

public class NextStepDbContext : DbContext
{
    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<Goal> Goals => Set<Goal>();

    public NextStepDbContext(DbContextOptions<NextStepDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NextStepDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
