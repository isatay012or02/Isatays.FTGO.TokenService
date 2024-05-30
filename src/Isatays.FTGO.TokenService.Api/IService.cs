using KDS.Primitives.FluentResult;

namespace Isatays.FTGO.TokenService.Api;

public interface IService
{
    Task<Result<string>> GetRoleCodeByCheckUser(string userName, string password);
}