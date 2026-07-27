namespace BookStore.Domain.Common.Primitives;

/// <summary>
/// Represents a Value Object in Domain-Driven Design.
/// Equality is determined by the values of its components,
/// not by object identity.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Returns all components that participate in equality comparison.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(
        ValueObject? left,
        ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        ValueObject? left,
        ValueObject? right)
    {
        return !Equals(left, right);
    }
}