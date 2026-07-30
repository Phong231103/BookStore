using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.Events
{
    public sealed class UserReactivatedDomainEvent : DomainEvent
    {
        public UserReactivatedDomainEvent(UserId userId)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}
