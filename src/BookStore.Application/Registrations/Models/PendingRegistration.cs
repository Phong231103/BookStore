using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Registration.Common;

public sealed class PendingRegistration
{
    private PendingRegistration()
    {
    }

    public Guid RegistrationId { get; init; }

    public required Email Email { get; init; } = default!;

    public required PasswordHash PasswordHash { get; init; }

    public required FullName FullName { get; init; }

    public PhoneNumber? PhoneNumber { get; init; }

    public required string OtpHash { get; init; }

    public int OtpAttemptCount { get; private set; }

    public DateTime ExpiredAtUtc { get; init; }

    public static PendingRegistration Create(
        Guid registrationId,
        Email email,
        PasswordHash passwordHash,
        FullName fullName,
        PhoneNumber? phoneNumber,
        string otpHash,
        DateTime expiresAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(otpHash);

        return new PendingRegistration
        {
            RegistrationId = registrationId,
            Email = email,
            PasswordHash = passwordHash,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            OtpHash = otpHash,
            ExpiredAtUtc = expiresAtUtc,
            OtpAttemptCount = 0
        };
    }

    public void IncreaseOtpAttempt()
    {
        OtpAttemptCount++;
    }
}