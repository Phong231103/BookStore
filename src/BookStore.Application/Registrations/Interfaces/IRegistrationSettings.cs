namespace BookStore.Application.Registrations.Interfaces
{
    public interface IRegistrationSettings
    {
        TimeSpan Expiration { get; }
    }
}
