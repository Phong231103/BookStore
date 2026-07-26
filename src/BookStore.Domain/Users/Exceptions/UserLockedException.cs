using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class UserLockedException : DomainException
    {
        public UserLockedException()
            : base("User account is locked.")
        {
        }
    }
}
