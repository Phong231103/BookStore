namespace BookStore.Domain.Users
{
    public sealed class UserRole : Entity
    {
        public Guid RoleId { get; private set; }

        public DateTime AssignedAt { get; private set; }

        private UserRole()
        {
            // EF Core
        }

        private UserRole(Guid roleId)
        {
            RoleId = roleId;
            AssignedAt = DateTime.UtcNow;
        }

        internal static UserRole Create(Guid roleId)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentException(nameof(roleId));

            return new UserRole(roleId);
        }
    }
}
