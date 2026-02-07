using System;

namespace ExampleProject.Core.Entities
{
    public class FlexibilityOffer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
