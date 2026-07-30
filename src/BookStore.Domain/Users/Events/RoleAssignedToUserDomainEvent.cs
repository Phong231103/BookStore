using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.Events
{
    public sealed class RoleAssignedToUserDomainEvent : DomainEvent
    {
        public RoleAssignedToUserDomainEvent(
            UserId userId,
            RoleId roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserId UserId { get; }

        public RoleId RoleId { get; }
    }
}
