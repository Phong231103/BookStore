using BookStore.Domain.Common.Primitives;

namespace BookStore.Domain.Users.Exceptions
{
    public sealed class DuplicateUserRoleException : DomainException
    {
        public DuplicateUserRoleException(Guid roleId)
            : base($"Role '{roleId}' has already been assigned.")
        {
        }
    }
}
