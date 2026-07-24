using BookStore.Domain.Common;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidUserIdException : DomainException
    {
        public InvalidUserIdException()
            : base("UserId cannot be empty.")
        {
        }
    }
}
