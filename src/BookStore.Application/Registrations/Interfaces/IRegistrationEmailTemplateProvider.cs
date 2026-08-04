using BookStore.Application.Registrations.Models;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Registrations.Interfaces
{
    public interface IRegistrationEmailTemplateProvider
    {
        EmailMessage CreateOtpEmail(Email recipient, string otp, DateTime expiresAtUtc);
    }
}
