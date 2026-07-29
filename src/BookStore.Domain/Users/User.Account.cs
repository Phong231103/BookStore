namespace BookStore.Domain.Users
{
    public sealed partial class User
    {
        /// <summary>
        /// Confirms the user's email address.
        /// </summary>
        public void ConfirmEmail(DateTime confirmedAtUtc)
        {
            if (EmailConfirmed)
                return;

            EmailConfirmed = true;

            UpdatedOnUtc = confirmedAtUtc;

            AddDomainEvent(
                new UserEmailConfirmedDomainEvent(
                    Id));
        }
    }
}
