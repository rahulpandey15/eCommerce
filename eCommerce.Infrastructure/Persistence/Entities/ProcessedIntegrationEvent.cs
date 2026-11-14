using System.ComponentModel.DataAnnotations;

namespace eCommerce.Infrastructure.Persistence.Entities;

public sealed class ProcessedIntegrationEvent
{
    [Key]
    public Guid EventId { get; set; }
    public required string ConsumerName { get; set; }
    public DateTimeOffset ProcessedAtUtc { get; set; }
}
