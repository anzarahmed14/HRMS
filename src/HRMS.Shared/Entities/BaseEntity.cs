namespace HRMS.Shared.Entities;
public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}