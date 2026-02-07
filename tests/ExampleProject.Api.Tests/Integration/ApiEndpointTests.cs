using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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

    [Fact]
    public async Task Get_ApiOffers_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/offers");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Post_ApiOffers_Returns_Created_And_Offer()
    {
        var response = await _client.PostAsJsonAsync("/api/offers", new { Name = "Test Offer", Status = "Active" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal("Test Offer", doc.GetProperty("name").GetString());
        Assert.Equal("Active", doc.GetProperty("status").GetString());
        Assert.True(doc.TryGetProperty("id", out _));
    }

    [Fact]
    public async Task Get_ApiAuditRecent_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/audit/recent?count=5");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Post_ApiAudit_Returns_Created_And_Entry()
    {
        var response = await _client.PostAsJsonAsync("/api/audit", new { Action = "TestAction", UserId = "user-1", Details = "Test details" });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal("TestAction", doc.GetProperty("action").GetString());
        Assert.Equal("user-1", doc.GetProperty("userId").GetString());
    }

    [Fact]
    public async Task Get_ApiPlants_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/plants");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Get_ApiMeterReadingsRecent_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/meter-readings/recent?count=10");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Get_ApiAlertsRecent_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/alerts/recent?count=10");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Get_ApiMarketSignalsRecent_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/market-signals/recent?count=10");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }

    [Fact]
    public async Task Get_ApiDispatchLogRecent_Returns_Ok_And_Array()
    {
        var response = await _client.GetAsync("/api/dispatch-log/recent?count=10");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<JsonElement>(json);
        Assert.Equal(JsonValueKind.Array, list.ValueKind);
    }
}
