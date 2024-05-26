namespace Isatays.FTGO.TokenService.Api.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) not found.")
    {
    }
}