namespace Ordering.Domain.Abstractions;

public interface IEntity
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}