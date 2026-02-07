using System;

namespace ExampleProject.Core.Entities
{
    /// <summary>Record of a flexibility dispatch (plant activated for a market).</summary>
    public class DispatchLogEntry
    {
        public string Id { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; }
        public Guid? PlantId { get; set; }
        public string PlantName { get; set; } = string.Empty;
        public decimal VolumeMw { get; set; }
        public string Market { get; set; } = string.Empty;
        public Guid? OfferId { get; set; }
        public string Direction { get; set; } = "Up"; // Up | Down
    }
}
