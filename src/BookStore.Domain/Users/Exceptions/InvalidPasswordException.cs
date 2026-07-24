using BookStore.Domain.Common;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException()
            : base("Password hash is invalid.")
        {
        }
    }
}
