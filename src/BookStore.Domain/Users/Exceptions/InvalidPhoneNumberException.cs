using BookStore.Domain.Common;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class InvalidPhoneNumberException : DomainException
    {
        public InvalidPhoneNumberException(string phone)
            : base($"'{phone}' is not a valid phone number.")
        {
        }
    }
}
