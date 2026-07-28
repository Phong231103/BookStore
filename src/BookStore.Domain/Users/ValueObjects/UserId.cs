using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.ValueObjects;

/// <summary>
/// Represents the unique identifier of a user.
/// </summary>
public sealed class UserId : ValueObject
{
    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="UserId"/> from the specified Guid.
    /// </summary>
    /// <param name="value">The Guid value.</param>
    /// <returns>A new <see cref="UserId"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the Guid is empty.
    /// </exception>
    public static UserId Create(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(value));

        return new UserId(value);
    }

    /// <summary>
    /// Creates a new unique <see cref="UserId"/>.
    /// </summary>
    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(UserId id)
    {
        return id.Value;
    }
}