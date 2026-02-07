using ExampleProject.Api.Services.Models;

namespace ExampleProject.Api.Services;

/// <summary>
/// Typed client for the app's REST API (offers = PostgreSQL, audit = MongoDB).
/// </summary>
public interface IApiClient
{
    Task<IReadOnlyList<OfferDto>> GetOffersAsync(CancellationToken cancellationToken = default);
    Task<OfferDto?> GetOfferAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OfferDto> CreateOfferAsync(CreateOfferDto request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken cancellationToken = default);
    Task<PlantDto?> GetPlantAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PlantDto> CreatePlantAsync(CreatePlantDto request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditEntryDto>> GetAuditRecentAsync(int count = 10, CancellationToken cancellationToken = default);
    Task<AuditEntryDto> LogAuditAsync(CreateAuditDto request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MarketSignalDto>> GetMarketSignalsRecentAsync(int count = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DispatchLogDto>> GetDispatchLogRecentAsync(int count = 20, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MeterReadingDto>> GetMeterReadingsRecentAsync(int count = 30, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AlertDto>> GetAlertsRecentAsync(int count = 20, CancellationToken cancellationToken = default);
}
