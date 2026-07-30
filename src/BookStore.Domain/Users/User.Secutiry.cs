using BookStore.Domain.Users.Enums;
using BookStore.Domain.Users.Exceptions;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Domain.Users
{
    public partial class User
    {
        public void ChangePassword(PasswordHash passwordHash, DateTime utcNow)
        {
            ArgumentNullException.ThrowIfNull(passwordHash);

            if (PasswordHash == passwordHash)
                throw new InvalidPasswordException();

            PasswordHash = passwordHash;

            Touch(utcNow);

            RaisePasswordChangedEvent();
        }

        public void RecordFailedLogin(int maxAttempts, TimeSpan lockoutDuration, DateTime utcNow)
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= maxAttempts)
            {
                LockUntil(utcNow.Add(lockoutDuration));

                RaiseLockedOutEvent();
            }

            Touch(utcNow);
        }

        public void RecordSuccessfulLogin(DateTime utcNow)
        {
            ResetFailedLoginState();
            Touch(utcNow);
        }

        public void EnableTwoFactor(TwoFactorMethod method, DateTime utcNow)
        {
            if (IsTwoFactorEnabled)
                return;

            TwoFactorMethod = method;

            Touch(utcNow);

            RaiseTwoFactorEnabledEvent(method);
        }

        public void DisableTwoFactor(DateTime utcNow)
        {
            if (!IsTwoFactorEnabled)
                return;

            TwoFactorMethod = null;

            Touch(utcNow);

            RaiseTwoFactorDisabledEvent();
        }
    }
}
