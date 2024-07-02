using Isatays.FTGO.TokenService.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api.Features.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Ftgo")!;

        services.AddDbContext<Context>(options =>
        {
            options.UseNpgsql(connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        3,
                        TimeSpan.FromSeconds(10),
                        null);
                });
        });

        return services;
    }
    
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddScoped<Service>();

        return services;
    }
}