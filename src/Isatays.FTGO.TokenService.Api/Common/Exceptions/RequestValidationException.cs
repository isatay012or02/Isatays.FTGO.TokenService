using FluentValidation;
using FluentValidation.Results;

namespace Isatays.FTGO.TokenService.Api.Common.Exceptions;

/// <summary>Описывает проблему валидации полей запроса</summary>
public class RequestValidationException : ValidationException
{
#pragma warning disable CS0108, CS0114
    public IDictionary<string, string[]> Errors { get; }
#pragma warning restore CS0108, CS0114

    /// <inheritdoc />
    public RequestValidationException() : base("Ошибка во время валидации тела запроса") =>
        Errors = new Dictionary<string, string[]>();

    /// <inheritdoc />
    public RequestValidationException(IEnumerable<ValidationFailure> failures) : this() =>
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
}