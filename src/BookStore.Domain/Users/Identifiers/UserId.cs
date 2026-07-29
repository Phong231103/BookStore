using BookStore.Domain.Common.Identifiers;

namespace BookStore.Domain.Users.Identifiers;

/// <summary>
/// Represents the unique identifier of a user.
/// </summary>
public sealed class UserId : StronglyTypedId
{
    private UserId(Guid value)
        : base(value)
    {
    }

    /// <summary>
    /// Creates a <see cref="UserId"/> from the specified Guid.
    /// </summary>
    public static UserId Create(Guid value)
        => new(value);

    /// <summary>
    /// Creates a new unique <see cref="UserId"/>.
    /// </summary>
    public static UserId New()
        => new(Guid.NewGuid());

    public static implicit operator Guid(UserId id)
        => id.Value;
}