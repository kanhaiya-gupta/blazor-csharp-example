using System;

namespace ExampleProject.Core.Entities
{
    /// <summary>Detected anomaly or alert (e.g. spike, threshold exceeded).</summary>
    public class Alert : BaseEntity
    {
        public Guid? PlantId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; }
        public DateTimeOffset? ResolvedAt { get; set; }
    }
}
