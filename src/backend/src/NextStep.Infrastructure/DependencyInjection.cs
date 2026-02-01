using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextStep.Application.Common;
using NextStep.Infrastructure.Persistence;
using NextStep.Infrastructure.Persistence.Repositories;

namespace NextStep.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<NextStepDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddScoped<IAthleteRepository, AthleteRepository>();

        return services;
    }
}
