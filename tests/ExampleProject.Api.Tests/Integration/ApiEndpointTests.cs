using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ExampleProject.Api.Tests.Integration;

/// <summary>
/// Integration tests for the API endpoints.
/// </summary>
public class ApiEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Root_Returns_Success()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("ExampleProject", html);
    }

    [Fact]
    public async Task Get_Api_Returns_PlainText()
    {
        var response = await _client.GetAsync("/api");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("ExampleProject.Api", body);
    }

    [Fact]
    public async Task Get_Favicon_Returns_NoContent()
    {
        var response = await _client.GetAsync("/favicon.ico");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
