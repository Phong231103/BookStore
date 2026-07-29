using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users.ChildEntity
{
    public sealed class UserRole : Entity<RoleId>
    {
        private UserRole()
        {
        }

        private UserRole(
            RoleId roleId,
            DateTime assignedAt)
            : base(roleId)
        {
            AssignedAt = assignedAt;
        }

        /// <summary>
        /// Gets the identifier of the assigned role.
        /// </summary>
        public RoleId RoleId => Id;

        /// <summary>
        /// Gets the UTC time when the role was assigned.
        /// </summary>
        public DateTime AssignedAt { get; }

        /// <summary>
        /// Creates a new role assignment.
        /// </summary>
        public static UserRole Create(
            RoleId roleId,
            DateTime assignedAt)
        {
            ArgumentNullException.ThrowIfNull(roleId);

            return new UserRole(roleId, assignedAt);
        }
    }
}
