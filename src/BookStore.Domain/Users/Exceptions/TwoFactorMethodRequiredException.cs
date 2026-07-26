using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class TwoFactorMethodRequiredException : DomainException
    {
        public TwoFactorMethodRequiredException()
            : base("Two-factor method is required when enabling two-factor authentication.")
        {
        }
    }
}
