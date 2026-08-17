namespace HRMS.BuildingBlocks.Domain.Entities;
public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}