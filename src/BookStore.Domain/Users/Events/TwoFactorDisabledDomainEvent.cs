using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.Events
{
    public sealed class TwoFactorDisabledDomainEvent : DomainEvent
    {
        public TwoFactorDisabledDomainEvent(UserId userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public UserId UserId { get; }
    }
}
