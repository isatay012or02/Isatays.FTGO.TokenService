using System.Globalization;
using Isatays.FTGO.TokenService.Api.Common.Extensions;
using Serilog;

namespace Isatays.FTGO.TokenService.Api.Features.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        #region Exceptions

        _ = app.UseGlobalExceptionHandler();

        #endregion Exceptions

        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ti.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

        #endregion Swagger

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        return app;
    }
}