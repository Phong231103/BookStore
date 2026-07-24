using BookStore.Domain.Common;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class CannotRemoveLastRoleException : DomainException
    {
        public CannotRemoveLastRoleException()
            : base("A user must have at least one role.")
        {
        }
    }
}
