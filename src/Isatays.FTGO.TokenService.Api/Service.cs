using Isatays.FTGO.TokenService.Api.Common.Errors;
using Isatays.FTGO.TokenService.Api.Common.Exceptions;
using Isatays.FTGO.TokenService.Api.Data;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api;

public class Service(ILogger<Service> logger, TokenContext context) : IService
{
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
            if (roleCode == string.Empty)
            {
                logger.LogError($"Не удалось получить данные по запросу.");
                return Result.Failure<string>(DomainError.NotFound);
            }
        }
        catch (DatabaseException ex)
        {
            logger.LogError($"Ошибка на уровне базы данных. Описание: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            logger.LogError($"Ошибка при обращений на базу данных. Описание ошибки: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }

        return Result.Success(roleCode!);
    }
}