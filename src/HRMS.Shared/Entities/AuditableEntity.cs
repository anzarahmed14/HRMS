namespace HRMS.Shared.Entities;

public abstract class AuditableEntity<TKey> : BaseEntity<TKey>
{
    public Guid? CreatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public bool IsDeleted { get; set; }
}