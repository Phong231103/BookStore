using BookStore.Application.Registrations.Interfaces;
using BookStore.Application.Registrations.Models;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Infrastructure.EmailSetting.Templates
{
    internal sealed class RegistrationEmailTemplateProvider
    : IRegistrationEmailTemplateProvider
    {
        public EmailMessage CreateOtpEmail(
            Email recipient,
            string otp,
            DateTime expiresAtUtc)
        {
            return new EmailMessage
            {
                To = recipient,
                Subject = "Verify your BookStore account",
                Body =
                        $"""
                    Hello,
                    
                    Your verification code is:
                    
                    {otp}
                    
                    This code will expire at {expiresAtUtc:yyyy-MM-dd HH:mm:ss} UTC.
                    
                    If you did not request this registration, you can safely ignore this email.
                    
                    BookStore Team
                    """
            };
        }
    }
}
