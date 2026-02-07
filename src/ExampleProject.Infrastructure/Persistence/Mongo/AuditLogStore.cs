using ExampleProject.Core.Entities;
using ExampleProject.Core.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ExampleProject.Infrastructure.Persistence.Mongo
{
    public class MongoAuditLogStore : IAuditLogStore
    {
        private readonly IMongoCollection<AuditDocument> _collection;

        public MongoAuditLogStore(IOptions<MongoOptions> options)
        {
            var o = options.Value;
            var client = new MongoClient(o.ConnectionString);
            var db = client.GetDatabase(o.DatabaseName);
            _collection = db.GetCollection<AuditDocument>(o.AuditCollectionName);
        }

        public async Task AppendAsync(AuditEntry entry, CancellationToken cancellationToken = default)
        {
            var doc = new AuditDocument
            {
                Action = entry.Action,
                UserId = entry.UserId,
                TimestampUtc = entry.Timestamp.UtcDateTime,
                Details = entry.Details
            };
            await _collection.InsertOneAsync(doc, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<AuditEntry>> GetRecentAsync(int count, CancellationToken cancellationToken = default)
        {
            var cursor = await _collection
                .Find(FilterDefinition<AuditDocument>.Empty)
                .SortByDescending(d => d.TimestampUtc)
                .Limit(count)
                .ToListAsync(cancellationToken);
            return cursor.Select(d => new AuditEntry
            {
                Id = d.Id.ToString(),
                Action = d.Action ?? string.Empty,
                UserId = d.UserId ?? string.Empty,
                Timestamp = d.TimestampUtc == default ? DateTimeOffset.UtcNow : new DateTimeOffset(DateTime.SpecifyKind(d.TimestampUtc, DateTimeKind.Utc)),
                Details = d.Details ?? string.Empty
            }).ToList();
        }
    }

    [BsonIgnoreExtraElements]
    internal class AuditDocument
    {
        [BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public string? Action { get; set; }
        public string? UserId { get; set; }
        /// <summary>BSON DateTime (UTC); key "Timestamp" matches Python population script.</summary>
        [BsonElement("Timestamp")]
        public DateTime TimestampUtc { get; set; }
        public string? Details { get; set; }
    }

    public class MongoOptions
    {
        public const string SectionName = "Mongo";
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = "exampleproject";
        public string AuditCollectionName { get; set; } = "audit";
        public string MarketSignalsCollectionName { get; set; } = "market_signals";
        public string DispatchLogCollectionName { get; set; } = "dispatch_log";
    }
}
