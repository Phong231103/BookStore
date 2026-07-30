using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Domain.Users.Events
{
    /// <summary>
    /// Raised when a new user is successfully registered.
    /// </summary>
    public sealed class UserRegisteredDomainEvent : DomainEvent
    {
        public UserRegisteredDomainEvent(
            UserId userId,
            Email email)
        {
            UserId = userId;
            Email = email;
        }

        /// <summary>
        /// Gets the registered user's identifier.
        /// </summary>
        public UserId UserId { get; }

        /// <summary>
        /// Gets the registered user's email address.
        /// </summary>
        public Email Email { get; }
    }
}
