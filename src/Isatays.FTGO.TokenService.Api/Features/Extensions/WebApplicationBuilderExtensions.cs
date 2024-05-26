using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Isatays.FTGO.TokenService.Api.Features.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        #region Logging

        //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
        //builder.Logging.ClearProviders();
        //builder.Logging.AddSerilog(Log.Logger);

        _ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
        {
            var assembly = Assembly.GetEntryAssembly();

            _ = loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("Assembly Version", assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version)
                    .Enrich.WithProperty("Assembly Informational Version", assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);
        });

        #endregion Logging

        #region Serialisation

        _ = builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        #endregion Serialisation

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "V1",
                Title = $"{ti.ToTitleCase(builder.Environment.EnvironmentName)} API",
                Description = "An API to show an implementation of AccountService.",
                Contact = new OpenApiContact
                {
                    Name = "Isatay Abdrakhmanov",
                    Email = "isaa012or02@gmail.com"
                }
            });
            c.TagActionsBy(api => new[] { api.GroupName });
            c.DocInclusionPredicate((name, api) => true);
        });

        #endregion Swagger

        #region Project Dependencies

        _ = builder.Services.ConfigureDatabaseConnection(builder.Configuration);
        _ = builder.Services.ConfigureDependencyInjection();

        #endregion Project Dependencies

        return builder;
    }
}