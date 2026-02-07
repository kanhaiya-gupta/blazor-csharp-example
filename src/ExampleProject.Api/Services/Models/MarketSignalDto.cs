namespace ExampleProject.Api.Services.Models;

public record MarketSignalDto(string Id, DateTimeOffset Timestamp, string Market, decimal? PriceEurPerMwh, decimal? VolumeMw, string? Region);
