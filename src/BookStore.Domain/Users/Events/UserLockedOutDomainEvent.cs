using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.Events
{
    public sealed class UserLockedOutDomainEvent : DomainEvent
    {
        public UserLockedOutDomainEvent(
            UserId userId,
            DateTime lockoutEndUtc)
        {
            UserId = userId;
            LockoutEndUtc = lockoutEndUtc;
        }

        public UserId UserId { get; }

        public DateTime LockoutEndUtc { get; }
    }
}
