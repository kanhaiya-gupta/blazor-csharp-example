namespace ExampleProject.Api.Services.Models;

public record AlertDto(Guid Id, Guid? PlantId, string Type, string Severity, string Message, DateTimeOffset Timestamp, DateTimeOffset? ResolvedAt);
