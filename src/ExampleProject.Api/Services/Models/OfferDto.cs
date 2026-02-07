namespace ExampleProject.Api.Services.Models;

public record OfferDto(Guid Id, string Name, string Status, DateTimeOffset CreatedAt);

/// <summary>Mutable form model for creating an offer (Blazor two-way binding).</summary>
public class CreateOfferDto
{
    public string Name { get; set; } = "";
    public string? Status { get; set; }
}
