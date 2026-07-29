using BookStore.Domain.Common.Identifiers;

namespace BookStore.Domain.Common.Primitives;

/// <summary>
/// Represents an entity in Domain-Driven Design.
/// An entity is uniquely identified by its identity rather than its attributes.
/// </summary>
public abstract class Entity<TId>
    where TId : StronglyTypedId
{
    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the identity of the entity.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    public sealed override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not Entity<TId> other)
            return false;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public sealed override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    public static bool operator ==(
        Entity<TId>? left,
        Entity<TId>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        Entity<TId>? left,
        Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}