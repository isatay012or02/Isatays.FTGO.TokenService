using Serilog;

namespace Isatays.FTGO.TokenService.Api.Features.Extensions;

public static class LoggingExtensions
{
    /// <summary>
    /// Method adds logging
    /// Setup Serilog
    /// Setup ELK logging
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddSerilog();
            options.SetMinimumLevel(LogLevel.Information);
        });

        return services;
    }
}