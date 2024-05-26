using System.Net;
using Isatays.FTGO.TokenService.Api.Common.Exceptions;
using Isatays.FTGO.TokenService.Api.Common.Extensions;

namespace Isatays.FTGO.TokenService.Api.Features.Middlewares;

/// <summary>Код промежуточного слоя для глобольного отлова исключений</summary>
public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandleMiddleware> _logger;

    /// <summary>
    /// Создает экземпляр <see cref="ExceptionHandleMiddleware"/>
    /// </summary>
    /// <param name="next"></param>
    /// <param name="loggerFactory"></param>
    public ExceptionHandleMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionHandleMiddleware>();
    }

    /// <summary>Промежжуточный метод</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RequestValidationException ex)
        {
            var problemDetails = ex.GenerateProblemDetails(context: context,
                title: "Переданы невалидные параметры. См. детали...",
                statusCode: HttpStatusCode.BadRequest);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (NotFoundException ex)
        {
            var problemDetails = ex.GenerateProblemDetails(
                context: context,
                title: "Не удалось найти сущность по заданному ключу",
                statusCode: HttpStatusCode.NotFound);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (NotLoggingException ex)
        {
            var problemDetails = ex.GenerateProblemDetails(
                context: context,
                title: "Ошибка во время выполнения процесса",
                statusCode: HttpStatusCode.UnprocessableEntity);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "{Message}", "Ошибка во время выполнения процесса");

            var problemDetails = ex.GenerateProblemDetails(
                context: context,
                title: "Ошибка во время выполнения процесса. См. детали...",
                statusCode: HttpStatusCode.UnprocessableEntity);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (ServiceUnavailableException ex)
        {
            _logger.LogError(ex, "{Message}",
                $"Достигнут лимит запросов в сервис. Сервис временно недоступен...");

            var problemDetails = ex.GenerateProblemDetails(
                context: context,
                title: "Сервис временно недоступен. См. детали...",
                statusCode: HttpStatusCode.ServiceUnavailable);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}",
                "Необработанное исключение. Детали во вложении...");

            var problemDetails = ex.GenerateProblemDetails(
                context: context,
                title: "Ошибка во время исполнения метода. См. детали...",
                statusCode: HttpStatusCode.InternalServerError);

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}