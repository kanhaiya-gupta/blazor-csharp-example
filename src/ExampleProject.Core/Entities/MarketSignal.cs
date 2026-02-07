using System;

namespace ExampleProject.Core.Entities
{
    /// <summary>Incoming market/price signal (day-ahead, intraday, balancing).</summary>
    public class MarketSignal
    {
        public string Id { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; }
        public string Market { get; set; } = string.Empty;
        public decimal? PriceEurPerMwh { get; set; }
        public decimal? VolumeMw { get; set; }
        public string? Region { get; set; }
    }
}
