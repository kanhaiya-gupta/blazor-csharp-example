// Blazor host — minimal entry point; expand per Implementation_plan.md
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5000";
apiBaseUrl = apiBaseUrl.TrimEnd('/');

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/status", async (CancellationToken ct) =>
{
    var connected = false;
    try
    {
        using var client = new HttpClient { BaseAddress = new Uri(apiBaseUrl), Timeout = TimeSpan.FromSeconds(2) };
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
        var response = await client.GetAsync("/", ct);
        connected = response.IsSuccessStatusCode;
    }
    catch { /* unreachable */ }

    return new { apiBaseUrl, apiConnected = connected };
});

app.Run();
