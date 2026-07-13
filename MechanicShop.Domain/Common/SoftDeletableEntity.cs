using MechanicShop.Domain.Common.Results;

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

    public Result<Deleted> SoftDelete(DateTimeOffset utcNow, Guid? userId)
    {
        if (IsDeleted)
            return Error.NotFound("delete","object already deleted");

        IsDeleted = true;
        DeletedAtUtc = utcNow;
        DeletedByUserId = userId;
        return Result.Deleted;

    }

    public Result<Success> Restore()
    {
        if (!IsDeleted)
            return Error.Failure("delete.entity","the object not deleted yet");

        IsDeleted = false;
        DeletedAtUtc = null;
        DeletedByUserId = null;

        return Result.Success;
    }
}