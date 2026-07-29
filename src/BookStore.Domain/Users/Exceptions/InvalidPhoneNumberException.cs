using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidPhoneNumberException : DomainException
    {
        public InvalidPhoneNumberException()
            : base($"Invalid phone number.")
        {
        }
    }
}
