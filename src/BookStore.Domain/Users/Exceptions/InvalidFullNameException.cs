using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidFullNameException : DomainException
    {
        public InvalidFullNameException()
            : base($"Invalid name.")
        {
        }
    }
}
