using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Users
{
	public sealed partial class User
	{
		/// <summary>
		/// Assigns a role to the user.
		/// </summary>
		public void AssignRole(
			RoleId roleId,
			DateTime assignedAtUtc)
		{
			ArgumentNullException.ThrowIfNull(roleId);

			if (_roles.Any(x => x.RoleId == roleId))
				throw new DuplicateRoleException();

			_roles.Add(
				UserRole.Create(
					roleId,
					assignedAtUtc));

			UpdatedOnUtc = assignedAtUtc;

			AddDomainEvent(
				new RoleAssignedToUserDomainEvent(
					Id,
					roleId));
		}

		/// <summary>
		/// Revokes a role from the user.
		/// </summary>
		public void RevokeRole(
			RoleId roleId,
			DateTime revokedAtUtc)
		{
			ArgumentNullException.ThrowIfNull(roleId);

			var role = _roles.FirstOrDefault(x => x.RoleId == roleId);

			if (role is null)
				return;

			if (_roles.Count == 1)
				throw new CannotRemoveLastRoleException();

			_roles.Remove(role);

			UpdatedOnUtc = revokedAtUtc;
		}
	}
}
