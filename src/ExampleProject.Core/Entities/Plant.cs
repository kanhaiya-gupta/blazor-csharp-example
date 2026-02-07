using System;

namespace ExampleProject.Core.Entities
{
    /// <summary>Registered flexibility asset (CHP, battery, turbine, industrial load, etc.).</summary>
    public class Plant : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public decimal? CapacityMw { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset RegisteredAt { get; set; }
    }
}
