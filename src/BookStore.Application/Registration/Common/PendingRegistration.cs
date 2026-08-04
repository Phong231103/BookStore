using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Registration.Common;

public sealed record PendingRegistration
{
    public Guid RegistrationId { get; init; }

    public required Email Email { get; init; }

    public required PasswordHash PasswordHash { get; init; }

    public required FullName FullName { get; init; }

    public PhoneNumber? PhoneNumber { get; init; }

    public required string OtpHash { get; init; }

    public int OtpAttemptCount { get; init; }

    public DateTime ExpiredAtUtc { get; init; }
}