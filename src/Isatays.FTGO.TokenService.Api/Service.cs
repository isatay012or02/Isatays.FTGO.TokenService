using Isatays.FTGO.TokenService.Api.Common.Errors;
using Isatays.FTGO.TokenService.Api.Common.Exceptions;
using Isatays.FTGO.TokenService.Api.Data;
using Issatays.Ftgo.Logger;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api;

/// <summary>
/// 
/// </summary>
public class Service(ILoggerService<Service> logger, Context context)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<Result<string>> GetRoleCodeByCheckUser(string userName, string password)
    {
        string? roleCode;
        try
        {
            roleCode = await context.Users
                .Where(u => u.UserName == userName && u.Password == password)
                .Join(context.Roles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) => role.Code)
                .FirstOrDefaultAsync();
            if (roleCode is null or "")
            {
                logger.LogError(nameof(GetRoleCodeByCheckUser), -45367, "Data's not found",
                    "Data's not found from the database by the requests", new { userName }, null!);
                return Result.Failure<string>(DomainError.NotFound);
            }
        }
        catch (DatabaseException ex)
        {
            logger.LogError(nameof(GetRoleCodeByCheckUser), -45368, ex.Message,
                "Error's on the database level", new { userName }, null!);
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            logger.LogError(nameof(GetRoleCodeByCheckUser), -45369, ex.Message,
                "Error's on the total level", new { userName }, null!);
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }

        return Result.Success(roleCode);
    }
}