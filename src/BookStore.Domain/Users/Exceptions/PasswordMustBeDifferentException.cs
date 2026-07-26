using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class PasswordMustBeDifferentException : DomainException
    {
        public PasswordMustBeDifferentException()
            : base("The new password must be different from the current password.")
        {
        }
    }
}
