using System.Diagnostics.CodeAnalysis;

namespace Isatays.FTGO.TokenService.Api.Common.Errors;

[ExcludeFromCodeCoverage]
public class ApiError
{
    public ApiError(string message, string? innerMessage, string? stackTrace)
    {
        Message = message;
        InnerMessage = innerMessage;
        StackTrace = stackTrace;
    }

    public string Message { get; set; }
    public string? InnerMessage { get; set; }
    public string? StackTrace { get; set; }
}