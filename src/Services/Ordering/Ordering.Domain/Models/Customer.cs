namespace Ordering.Domain.Models;

public class Customer : Entity<Guid>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}