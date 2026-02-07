using System.Net.Http.Json;
using ExampleProject.Api.Services.Models;

namespace ExampleProject.Api.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http) => _http = http;

    public async Task<IReadOnlyList<OfferDto>> GetOffersAsync(CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<OfferDto[]>("/api/offers", cancellationToken);
        return list ?? Array.Empty<OfferDto>();
    }

    public async Task<OfferDto?> GetOfferAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"/api/offers/{id}", cancellationToken);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<OfferDto>(cancellationToken)
            : null;
    }

    public async Task<OfferDto> CreateOfferAsync(CreateOfferDto request, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("/api/offers", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<OfferDto>(cancellationToken);
        return created!;
    }

    public async Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<PlantDto[]>("/api/plants", cancellationToken);
        return list ?? Array.Empty<PlantDto>();
    }

    public async Task<PlantDto?> GetPlantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"/api/plants/{id}", cancellationToken);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<PlantDto>(cancellationToken)
            : null;
    }

    public async Task<PlantDto> CreatePlantAsync(CreatePlantDto request, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("/api/plants", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<PlantDto>(cancellationToken);
        return created!;
    }

    public async Task<IReadOnlyList<AuditEntryDto>> GetAuditRecentAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<AuditEntryDto[]>($"/api/audit/recent?count={count}", cancellationToken);
        return list ?? Array.Empty<AuditEntryDto>();
    }

    public async Task<AuditEntryDto> LogAuditAsync(CreateAuditDto request, CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("/api/audit", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<AuditEntryDto>(cancellationToken);
        return created!;
    }

    public async Task<IReadOnlyList<MarketSignalDto>> GetMarketSignalsRecentAsync(int count = 20, CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<MarketSignalDto[]>($"/api/market-signals/recent?count={count}", cancellationToken);
        return list ?? Array.Empty<MarketSignalDto>();
    }

    public async Task<IReadOnlyList<DispatchLogDto>> GetDispatchLogRecentAsync(int count = 20, CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<DispatchLogDto[]>($"/api/dispatch-log/recent?count={count}", cancellationToken);
        return list ?? Array.Empty<DispatchLogDto>();
    }

    public async Task<IReadOnlyList<MeterReadingDto>> GetMeterReadingsRecentAsync(int count = 30, CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<MeterReadingDto[]>($"/api/meter-readings/recent?count={count}", cancellationToken);
        return list ?? Array.Empty<MeterReadingDto>();
    }

    public async Task<IReadOnlyList<AlertDto>> GetAlertsRecentAsync(int count = 20, CancellationToken cancellationToken = default)
    {
        var list = await _http.GetFromJsonAsync<AlertDto[]>($"/api/alerts/recent?count={count}", cancellationToken);
        return list ?? Array.Empty<AlertDto>();
    }
}
