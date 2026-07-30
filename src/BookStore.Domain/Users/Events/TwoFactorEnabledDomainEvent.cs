using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Enums;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.Events
{
    public sealed class TwoFactorEnabledDomainEvent : DomainEvent
    {
        public TwoFactorEnabledDomainEvent(
            UserId userId,
            TwoFactorMethod method)
        {
            UserId = userId;
            Method = method;
        }

        public UserId UserId { get; }

        public TwoFactorMethod Method { get; }
    }
}
