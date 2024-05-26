using System.Net.Mime;
using Isatays.FTGO.TokenService.Api.Common.Errors;
using Isatays.FTGO.TokenService.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Isatays.FTGO.TokenService.Api;

public static class Endpoint
{
    public static void ConfigureAccountEndpoints(this WebApplication app)
    {
        app.MapGet("api/v1", GetToken)
            .WithGroupName("Token")
            .Produces<string>(StatusCodes.Status200OK, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status400BadRequest, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status404NotFound, contentType: MediaTypeNames.Application.Json)
            .Produces<ApiError>(StatusCodes.Status500InternalServerError, contentType: MediaTypeNames.Application.Json);
    }
    
    private static async Task<IResult> GetToken(Service service, GetTokenDto request)
    {
        var result = await service.GetRoleCodeByCheckUser(request.UserName, request.Password);

        return Results.Ok(result.Value);
    }
}