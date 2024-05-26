namespace Isatays.FTGO.TokenService.Api.Common.Exceptions;

/// <summary>Базовое исключения для ошибок при выполнении бизнес логики</summary>
public abstract class DomainException : Exception
{
    /// <inheritdoc />
    public DomainException(string message) : base(message) { }

    /// <inheritdoc />
    public DomainException(string message, Exception ex) : base(message, ex) { }
}