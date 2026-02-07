using System;

namespace ExampleProject.Core.Entities
{
    /// <summary>Time-series reading (e.g. MW or kWh) per plant for analytics/anomaly detection.</summary>
    public class MeterReading : BaseEntity
    {
        public Guid? PlantId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal Value { get; set; }
        public string? MetricType { get; set; }
    }
}
