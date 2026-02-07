using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    public class MongoMarketSignalStore : IMarketSignalStore
    {
        private readonly IMongoCollection<MarketSignalDocument> _collection;

        public MongoMarketSignalStore(IOptions<MongoOptions> options)
        {
            var o = options.Value;
            var client = new MongoClient(o.ConnectionString);
            var db = client.GetDatabase(o.DatabaseName);
            _collection = db.GetCollection<MarketSignalDocument>(o.MarketSignalsCollectionName);
        }

        public async Task<IReadOnlyList<MarketSignal>> GetRecentAsync(int count, CancellationToken cancellationToken = default)
        {
            var cursor = await _collection
                .Find(FilterDefinition<MarketSignalDocument>.Empty)
                .SortByDescending(d => d.TimestampUtc)
                .Limit(count)
                .ToListAsync(cancellationToken);
            return cursor.Select(d => new MarketSignal
            {
                Id = d.Id.ToString(),
                Timestamp = d.TimestampUtc == default ? default : new DateTimeOffset(DateTime.SpecifyKind(d.TimestampUtc, DateTimeKind.Utc)),
                Market = d.Market ?? "",
                PriceEurPerMwh = d.PriceEurPerMwh,
                VolumeMw = d.VolumeMw,
                Region = d.Region
            }).ToList();
        }
    }

    [BsonIgnoreExtraElements]
    internal class MarketSignalDocument
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; }
        [BsonElement("Timestamp")]
        public DateTime TimestampUtc { get; set; }
        public string? Market { get; set; }
        public decimal? PriceEurPerMwh { get; set; }
        public decimal? VolumeMw { get; set; }
        public string? Region { get; set; }
    }
}
