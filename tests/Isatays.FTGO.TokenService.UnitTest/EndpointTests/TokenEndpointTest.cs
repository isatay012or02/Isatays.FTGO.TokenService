using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Isatays.FTGO.TokenService.Api;
using Isatays.FTGO.TokenService.Api.Models;
using KDS.Primitives.FluentResult;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace Isatays.FTGO.TokenService.UnitTest.EndpointTests;

public class TokenEndpointTest
{
    private readonly WebApplicationFactory<Program> _factory;

    public TokenEndpointTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateToken_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mockService = new Mock<Service>();
        mockService.Setup(s => s.GetRoleCodeByCheckUser(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Success("roleCode"));
        var request = new GetTokenDto("validUser", "validPassword");

        // Act
        var response = await client.PostAsJsonAsync("api/v1/token", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Token));
    }

    [Fact]
    public async Task CreateToken_ReturnsBadRequest_WhenCredentialsAreInvalid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mockService = new Mock<Service>();
        mockService.Setup(s => s.GetRoleCodeByCheckUser(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Failure<string>(new Error("", "")));
        var request = new GetTokenDto("invalidUser", "invalidPassword");

        // Act
        var response = await client.PostAsJsonAsync("api/v1/token", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private class TokenResponse
    {
        public string Token { get; set; }
    }
}