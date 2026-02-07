// Single app: Blazor UI + API on one host
using ExampleProject.Api.Components;
using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure;
using ExampleProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ExampleProject.Api.Services.IApiClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var configuredBase = config["App:ApiBaseUrl"];
    Uri baseUri;
    if (!string.IsNullOrWhiteSpace(configuredBase))
    {
        baseUri = new Uri(configuredBase.TrimEnd('/'));
    }
    else
    {
        var ctx = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
        baseUri = ctx != null
            ? new Uri($"{ctx.Request.Scheme}://{ctx.Request.Host}")
            : new Uri("http://localhost:5000");
    }
    var client = new HttpClient { BaseAddress = baseUri };
    return new ExampleProject.Api.Services.ApiClient(client);
});

var app = builder.Build();

// Ensure Postgres schema when real DB is configured (e.g. CI, first run). If the schema
// already exists (e.g. created by population/run_all.py), we use it and do not recreate.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetService<AppDbContext>();
    if (db != null)
    {
        try
        {
            db.Database.EnsureCreated();
        }
        catch (Exception ex) when (ex.Message.Contains("Failed to connect", StringComparison.OrdinalIgnoreCase)
            || ex.InnerException is System.Net.Sockets.SocketException)
        {
            Console.Error.WriteLine();
            Console.Error.WriteLine("Postgres is configured but not reachable. Start the database (e.g. scripts/start-databases.sh or docker-compose up -d postgres) or clear ConnectionStrings:DefaultConnection in appsettings to run without a database.");
            Environment.Exit(1);
        }
        catch (Exception ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            // Schema was created by population script or a previous run; use existing database.
        }
    }
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/favicon.ico", () => Results.NoContent());
app.MapGet("/api", () => "ExampleProject.Api");

// Offers (PostgreSQL)
app.MapGet("/api/offers", async (IFlexibilityOfferRepository repo, CancellationToken ct) =>
{
    var list = await repo.GetAllAsync(ct);
    return Results.Ok(list.Select(o => new { o.Id, o.Name, o.Status, o.CreatedAt }));
});
app.MapGet("/api/offers/{id:guid}", async (Guid id, IFlexibilityOfferRepository repo, CancellationToken ct) =>
{
    var offer = await repo.GetByIdAsync(id, ct);
    return offer is null ? Results.NotFound() : Results.Ok(new { offer.Id, offer.Name, offer.Status, offer.CreatedAt });
});
app.MapPost("/api/offers", async (CreateOfferRequest request, IFlexibilityOfferRepository repo, CancellationToken ct) =>
{
    var offer = new FlexibilityOffer
    {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Status = request.Status ?? "Pending",
        CreatedAt = DateTimeOffset.UtcNow
    };
    await repo.AddAsync(offer, ct);
    return Results.Created($"/api/offers/{offer.Id}", new { offer.Id, offer.Name, offer.Status, offer.CreatedAt });
});

// Plants (PostgreSQL – second table)
app.MapGet("/api/plants", async (IPlantRepository repo, CancellationToken ct) =>
{
    var list = await repo.GetAllAsync(ct);
    return Results.Ok(list.Select(p => new { p.Id, p.Name, p.AssetType, p.CapacityMw, p.Status, p.RegisteredAt }));
});
app.MapGet("/api/plants/{id:guid}", async (Guid id, IPlantRepository repo, CancellationToken ct) =>
{
    var plant = await repo.GetByIdAsync(id, ct);
    return plant is null ? Results.NotFound() : Results.Ok(new { plant.Id, plant.Name, plant.AssetType, plant.CapacityMw, plant.Status, plant.RegisteredAt });
});
app.MapPost("/api/plants", async (CreatePlantRequest request, IPlantRepository repo, CancellationToken ct) =>
{
    var plant = new Plant
    {
        Id = Guid.NewGuid(),
        Name = request.Name,
        AssetType = request.AssetType ?? "",
        CapacityMw = request.CapacityMw,
        Status = request.Status ?? "Pending",
        RegisteredAt = DateTimeOffset.UtcNow
    };
    await repo.AddAsync(plant, ct);
    return Results.Created($"/api/plants/{plant.Id}", new { plant.Id, plant.Name, plant.AssetType, plant.CapacityMw, plant.Status, plant.RegisteredAt });
});

// Audit (MongoDB)
app.MapGet("/api/audit/recent", async (IAuditLogStore store, int count = 10, CancellationToken ct = default) =>
{
    var list = await store.GetRecentAsync(count, ct);
    return Results.Ok(list.Select(e => new { e.Id, e.Action, e.UserId, e.Timestamp, e.Details }));
});
app.MapGet("/api/audit/status", async (IAuditLogStore store, CancellationToken ct) =>
{
    var list = await store.GetRecentAsync(100, ct);
    return Results.Json(new { store = store.GetType().Name, count = list.Count });
});
app.MapPost("/api/audit", async (CreateAuditRequest request, IAuditLogStore store, CancellationToken ct) =>
{
    var entry = new AuditEntry
    {
        Id = Guid.NewGuid().ToString(),
        Action = request.Action,
        UserId = request.UserId ?? "",
        Timestamp = DateTimeOffset.UtcNow,
        Details = request.Details ?? ""
    };
    await store.AppendAsync(entry, ct);
    return Results.Created("/api/audit/recent", new { entry.Id, entry.Action, entry.UserId, entry.Timestamp, entry.Details });
});

// Market signals (MongoDB) – incoming market/price data
app.MapGet("/api/market-signals/recent", async (IMarketSignalStore store, int count = 20, CancellationToken ct = default) =>
{
    var list = await store.GetRecentAsync(count, ct);
    return Results.Ok(list.Select(s => new { s.Id, s.Timestamp, s.Market, s.PriceEurPerMwh, s.VolumeMw, s.Region }));
});

// Dispatch log (MongoDB) – flexibility dispatch records
app.MapGet("/api/dispatch-log/recent", async (IDispatchLogStore store, int count = 20, CancellationToken ct = default) =>
{
    var list = await store.GetRecentAsync(count, ct);
    return Results.Ok(list.Select(d => new { d.Id, d.Timestamp, d.PlantId, d.PlantName, d.VolumeMw, d.Market, d.OfferId, d.Direction }));
});

// Meter readings (PostgreSQL) – time-series data for anomaly detection
app.MapGet("/api/meter-readings/recent", async (IMeterReadingRepository repo, int count = 30, CancellationToken ct = default) =>
{
    try
    {
        var list = await repo.GetRecentAsync(count, ct);
        return Results.Ok(list.Select(r => new { r.Id, r.PlantId, r.Timestamp, r.Value, r.MetricType }));
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Meter readings could not be loaded");
        return Results.Ok(Array.Empty<object>());
    }
});

// Alerts (PostgreSQL) – detected anomalies
app.MapGet("/api/alerts/recent", async (IAlertRepository repo, int count = 20, CancellationToken ct = default) =>
{
    var list = await repo.GetRecentAsync(count, ct);
    return Results.Ok(list.Select(a => new { a.Id, a.PlantId, a.Type, a.Severity, a.Message, a.Timestamp, a.ResolvedAt }));
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public record CreateOfferRequest(string Name, string? Status);
public record CreatePlantRequest(string Name, string? AssetType, decimal? CapacityMw, string? Status);
public record CreateAuditRequest(string Action, string? UserId, string? Details);
