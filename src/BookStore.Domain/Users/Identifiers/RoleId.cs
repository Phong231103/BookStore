using BookStore.Domain.Common.Identifiers;

namespace BookStore.Domain.Users.Identifiers
{
    public sealed class RoleId : StronglyTypedId
    {
        private RoleId(Guid value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates a role identifier from the specified Guid.
        /// </summary>
        public static RoleId Create(Guid value)
        {
            return new RoleId(value);
        }

        /// <summary>
        /// Creates a new unique role identifier.
        /// </summary>
        public static RoleId New()
        {
            return new RoleId(Guid.NewGuid());
        }

        public static implicit operator Guid(RoleId id)
        {
            return id.Value;
        }
    }
}
