namespace ExampleProject.Api.Services.Models;

public record AuditEntryDto(string Id, string Action, string UserId, DateTimeOffset Timestamp, string Details);

/// <summary>Mutable form model for logging audit (Blazor two-way binding).</summary>
public class CreateAuditDto
{
    public string Action { get; set; } = "";
    public string? UserId { get; set; }
    public string? Details { get; set; }
}
