using Isatays.FTGO.TokenService.Api.Common.Errors;
using Isatays.FTGO.TokenService.Api.Common.Exceptions;
using Isatays.FTGO.TokenService.Api.Data;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;

namespace Isatays.FTGO.TokenService.Api;

public class Service
{
    private readonly ILogger<Service> _logger;
    private readonly TokenContext _context;

    public Service(ILogger<Service> logger, TokenContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<Result<string>> GetRoleCodeByCheckUser(string userName, string password)
    {
        string? roleCode;
        try
        {
            roleCode = await _context.Users
                .Where(u => u.UserName == userName && u.Password == password)
                .Join(_context.Roles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) => role.Code)
                .FirstOrDefaultAsync();
            if (roleCode == string.Empty)
            {
                _logger.LogError($"Не удалось получить данные по запросу.");
                return Result.Failure<string>(DomainError.NotFound);
            }
        }
        catch (DatabaseException ex)
        {
            _logger.LogError($"Ошибка на уровне базы данных. Описание: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при обращений на базу данных. Описание ошибки: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }

        return Result.Success(roleCode!);
    }
}