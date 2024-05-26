namespace Isatays.FTGO.TokenService.Api.Common.Exceptions;

public abstract class ServiceUnavailableException : Exception
{
    /// <inheritdoc />
    protected ServiceUnavailableException(string method) : base(method) { }

    /// <inheritdoc />
    protected ServiceUnavailableException(string method, Exception ex) : base(method, ex) { }
}