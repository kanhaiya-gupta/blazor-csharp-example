namespace ExampleProject.Api.Services.Models;

public record MeterReadingDto(Guid Id, Guid? PlantId, DateTimeOffset Timestamp, decimal Value, string? MetricType);
