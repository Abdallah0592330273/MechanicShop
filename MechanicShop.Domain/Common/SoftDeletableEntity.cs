namespace MechanicShop.Domain.Common.Entities;

public abstract class SoftDeletableEntity : AuditableEntity
{
    protected SoftDeletableEntity(Guid id) : base(id)
    {
    }

    protected SoftDeletableEntity()
    {
        // For EF Core
    }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAtUtc { get; private set; }

    public Guid? DeletedByUserId { get; private set; }

    public void SoftDelete(DateTimeOffset utcNow, Guid? userId)
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedAtUtc = utcNow;
        DeletedByUserId = userId;
    }

    public void Restore()
    {
        if (!IsDeleted)
            return;

        IsDeleted = false;
        DeletedAtUtc = null;
        DeletedByUserId = null;
    }
}