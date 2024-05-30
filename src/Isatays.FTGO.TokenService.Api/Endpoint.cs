using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using Isatays.FTGO.TokenService.Api.Common.Errors;
using Isatays.FTGO.TokenService.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace Isatays.FTGO.TokenService.Api;

public static class Endpoint
{
    public static void ConfigureAccountEndpoints(this WebApplication app)
    {
        app.MapPost("api/v1", CreateToken)
            .WithGroupName("Token")
            .Produces<string>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);
    }
    
    private static async Task<IResult> CreateToken(IService service, IConfiguration configuration, [FromBody] GetTokenDto request)
    {
        var roleCode = await service.GetRoleCodeByCheckUser(request.UserName, request.Password);
        if (roleCode.IsFailed)
        {
            return Results.BadRequest("Не корректный имя пользователя или пароль");
        }
                
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = configuration["AuthOptions:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenLifeExpiration = int.Parse(configuration["AuthOptions:TokenLifeExpirationInHour"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, request.UserName),
                new(ClaimTypes.Role, configuration["AuthOptions:RoleCode"])
            }),
            // Время жизни токена
            Expires = DateTime.UtcNow.AddHours(tokenLifeExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        return Results.Ok(new { Token = tokenString });
    }
}