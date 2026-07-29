using BookStore.Domain.Users.ChildEntity;
using BookStore.Domain.Users.Exceptions;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Domain.Users
{
    public sealed partial class User
    {
        /// <summary>
        /// Assigns a role to the user.
        /// </summary>
        public void AssignRole(RoleId roleId, DateTime utcNow)
        {
            ArgumentNullException.ThrowIfNull(roleId);

            if (HasRole(roleId))
                throw new DuplicateRoleException();

            AddRole(UserRole.Create(roleId, utcNow));

            Touch(utcNow);

            RaiseRoleAssignedEvent(roleId);
        }

        /// <summary>
        /// Revokes a role from the user.
        /// </summary>
        public void RevokeRole(RoleId roleId, DateTime utcNow)
        {
            ArgumentNullException.ThrowIfNull(roleId);

            var role = FindRole(roleId);

            if (role is null)
                return;

            if (_roles.Count == 1)
                throw new CannotRemoveLastRoleException();

            RemoveRole(role);

            Touch(utcNow);

            RaiseRoleRevokedEvent(roleId);
        }
    }
}
