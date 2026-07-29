using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidEmailException : DomainException
    {
        public InvalidEmailException()
            : base($"Invalid email address.")
        {
        }
    }
}
