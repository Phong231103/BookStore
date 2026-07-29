using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.ChildEntity;
using BookStore.Domain.Users.Enums;
using BookStore.Domain.Users.Identifiers;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Domain.Users
{
    /// <summary>
    /// Represents the aggregate root for a system user.
    /// </summary>
    public sealed partial class User : AggregateRoot<UserId>
    {
        private readonly List<UserRole> _roles = [];

        private User()
        {
        }

        private User(
            UserId id,
            Email email,
            PasswordHash passwordHash,
            FullName fullName,
            PhoneNumber phoneNumber,
            DateTime createdAt)
            : base(id)
        {
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            PhoneNumber = phoneNumber;

            Status = UserStatus.Active;

            EmailConfirmed = false;

            TwoFactorEnabled = false;
            TwoFactorMethod = null;

            FailedLoginAttempts = 0;
            LockoutEndUtc = null;

            CreatedOnUtc = createdAt;
            UpdatedOnUtc = createdAt;
        }

        public Email Email { get; private set; }

        public PasswordHash PasswordHash { get; private set; }

        public FullName FullName { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public UserStatus Status { get; private set; }

        public bool EmailConfirmed { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public TwoFactorMethod? TwoFactorMethod { get; private set; }

        public int FailedLoginAttempts { get; private set; }

        public DateTime? LockoutEndUtc { get; private set; }

        public DateTime CreatedOnUtc { get; }

        public DateTime UpdatedOnUtc { get; private set; }

        public IReadOnlyCollection<UserRole> Roles
            => _roles.AsReadOnly();

        /// <summary>
        /// Registers a new user.
        /// </summary>
        public static User Register(
            UserId id,
            Email email,
            PasswordHash passwordHash,
            FullName fullName,
            PhoneNumber phoneNumber,
            RoleId defaultRoleId,
            DateTime createdAt)
        {
            ArgumentNullException.ThrowIfNull(id);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(passwordHash);
            ArgumentNullException.ThrowIfNull(fullName);
            ArgumentNullException.ThrowIfNull(phoneNumber);
            ArgumentNullException.ThrowIfNull(defaultRoleId);

            var user = new User(
                id,
                email,
                passwordHash,
                fullName,
                phoneNumber,
                createdAt);

            user.AddRole(
                UserRole.Create(
                    defaultRoleId,
                    createdAt));

            user.RaiseRegisteredEvent();

            return user;
        }

        private void Touch(DateTime utcNow)
        {
            UpdatedOnUtc = utcNow;
        }

        private bool HasRole(RoleId roleId)
        {
            return _roles.Any(x => x.RoleId == roleId);
        }

        private UserRole? FindRole(RoleId roleId)
        {
            return _roles.FirstOrDefault(x => x.RoleId == roleId);
        }

        private void AddRole(UserRole role)
        {
            _roles.Add(role);
        }

        private void RemoveRole(UserRole role)
        {
            _roles.Remove(role);
        }

        private void RaiseRegisteredEvent()
        {
            AddDomainEvent(
                new UserRegisteredDomainEvent(Id));
        }
    }
}
