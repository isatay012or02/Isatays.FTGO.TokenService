using System.Net;
using System.Reflection;
using Isatays.FTGO.TokenService.Api.Common.Constants;
using Isatays.FTGO.TokenService.Api.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Isatays.FTGO.TokenService.Api.Common.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Формирует экземпляр класса <see cref="ProblemDetails"/> для ответа в случае исключения
    /// Шаблонизирует ответ для единого формата ответов
    /// Добавляет статус код ответа
    /// Добавляет тип содержания appication/problem+json
    /// Добавляет текст заголовка ответа
    /// Добавляет детали ошибки
    /// Добавляет название проекта из которого выброшено исключение
    /// </summary>
    internal static ProblemDetails GenerateProblemDetails(this Exception ex,
        HttpContext context,
        string title,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        ProblemDetails problemDetails = new()
        {
            Status = (int)statusCode,
            Title = title,
            Detail = ex.Message,
            Instance = Assembly.GetExecutingAssembly().GetName().Name?.ToLower(),
        };

        if (ex is RequestValidationException requestValidationException)
            problemDetails.Extensions.Add("errors",
                requestValidationException.Errors.SelectMany(x => x.Value));

        problemDetails.Extensions.Add("requested_with", context.Request.Headers[HeaderConstant.RequestedWith].ToString());
        return problemDetails;
    }
}