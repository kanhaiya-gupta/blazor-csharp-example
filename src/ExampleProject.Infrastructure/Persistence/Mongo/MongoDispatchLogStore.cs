using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    public class MongoDispatchLogStore : IDispatchLogStore
    {
        private readonly IMongoCollection<DispatchLogDocument> _collection;

        public MongoDispatchLogStore(IOptions<MongoOptions> options)
        {
            var o = options.Value;
            var client = new MongoClient(o.ConnectionString);
            var db = client.GetDatabase(o.DatabaseName);
            _collection = db.GetCollection<DispatchLogDocument>(o.DispatchLogCollectionName);
        }

        public async Task<IReadOnlyList<DispatchLogEntry>> GetRecentAsync(int count, CancellationToken cancellationToken = default)
        {
            var cursor = await _collection
                .Find(FilterDefinition<DispatchLogDocument>.Empty)
                .SortByDescending(d => d.TimestampUtc)
                .Limit(count)
                .ToListAsync(cancellationToken);
            return cursor.Select(d => new DispatchLogEntry
            {
                Id = d.Id.ToString(),
                Timestamp = d.TimestampUtc == default ? default : new DateTimeOffset(DateTime.SpecifyKind(d.TimestampUtc, DateTimeKind.Utc)),
                PlantId = d.PlantId,
                PlantName = d.PlantName ?? "",
                VolumeMw = d.VolumeMw,
                Market = d.Market ?? "",
                OfferId = d.OfferId,
                Direction = d.Direction ?? "Up"
            }).ToList();
        }
    }

    [BsonIgnoreExtraElements]
    internal class DispatchLogDocument
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; }
        [BsonElement("Timestamp")]
        public DateTime TimestampUtc { get; set; }
        public Guid? PlantId { get; set; }
        public string? PlantName { get; set; }
        public decimal VolumeMw { get; set; }
        public string? Market { get; set; }
        public Guid? OfferId { get; set; }
        public string? Direction { get; set; }
    }
}
