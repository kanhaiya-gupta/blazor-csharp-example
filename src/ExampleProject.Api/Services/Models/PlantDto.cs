namespace ExampleProject.Api.Services.Models;

public record PlantDto(Guid Id, string Name, string AssetType, decimal? CapacityMw, string Status, DateTimeOffset RegisteredAt);

/// <summary>Mutable form model for creating a plant (Blazor two-way binding).</summary>
public class CreatePlantDto
{
    public string Name { get; set; } = "";
    public string AssetType { get; set; } = "";
    public decimal? CapacityMw { get; set; }
    public string? Status { get; set; }
}
