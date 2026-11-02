namespace Identity.API.Models;

public sealed class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string TenantId { get; set; }

    public required string ObjectId { get; set; }

    public required string Issuer { get; set; }

    public required string Subject { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}