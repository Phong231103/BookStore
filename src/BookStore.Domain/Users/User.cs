using BookStore.Domain.Users.Enums;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Domain.Users
{
    public sealed partial class User : AggregateRoot<UserId>
    {
        private readonly List<UserRole> _roles = [];

        public Email Email { get; private set; }

        public PasswordHash PasswordHash { get; private set; }

        public PhoneNumber? PhoneNumber { get; private set; }

        public FullName FullName { get; private set; }

        public UserStatus Status { get; private set; }

        public bool EmailConfirmed { get; private set; }

        public int FailedLoginAttempts { get; private set; }

        public DateTime? LockedUntil { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public TwoFactorMethod? TwoFactorMethod { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyCollection<UserRole> Roles
            => _roles.AsReadOnly();

        private User()
        {
        }

        private User(
            UserId id,
            Email email,
            PasswordHash passwordHash,
            FullName fullName)
            : base(id)
        {
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;

            Status = UserStatus.PendingVerification;

            EmailConfirmed = false;

            FailedLoginAttempts = 0;

            TwoFactorEnabled = false;

            CreatedAt = DateTime.UtcNow;

            UpdatedAt = CreatedAt;
        }

        public static User Register(Email email, PasswordHash passwordHash, FullName fullName, Guid defaultRoleId)
        {
            var user = new User(UserId.New(), email, passwordHash, fullName);

            user.AssignRole(defaultRoleId);

            user.RaiseDomainEvent(
                new UserRegisteredDomainEvent(user.Id));

            return user;
        }
    }
}
