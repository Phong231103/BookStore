using BookStore.Domain.Users.Enums;

namespace BookStore.Domain.Users
{
    public partial class User
    {
        public void Deactivate(DateTime utcNow)
        {
            if (Status == UserStatus.Deactivated)
                return;

            Status = UserStatus.Deactivated;

            Touch(utcNow);
        }

        public void Reactivate(DateTime utcNow)
        {
            if (Status == UserStatus.Active)
                return;

            Status = UserStatus.Active;

            Touch(utcNow);
        }
    }
}
