using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string email)
            : base($"'{email}' is not a valid email address.")
        {
        }
    }
}
