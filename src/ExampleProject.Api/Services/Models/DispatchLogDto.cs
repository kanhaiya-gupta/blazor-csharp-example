namespace ExampleProject.Api.Services.Models;

public record DispatchLogDto(string Id, DateTimeOffset Timestamp, Guid? PlantId, string PlantName, decimal VolumeMw, string Market, Guid? OfferId, string Direction);
