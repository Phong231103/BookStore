using BookStore.Domain.Common;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class UserInactiveException : DomainException
    {
        public UserInactiveException()
            : base("User account is not active.")
        {
        }
    }
}
