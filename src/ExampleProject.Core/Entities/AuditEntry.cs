using System;

namespace ExampleProject.Core.Entities
{
    public class AuditEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}
