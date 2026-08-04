namespace BookStore.Application.Registrations.StartRegistration
{
    public sealed record StartRegistrationResponse
    {
        public required Guid RegistrationId { get; init; }

        public required DateTime ExpiresAtUtc { get; init; }
    }
}
