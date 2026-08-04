using BookStore.Application.Registrations.Models;

namespace BookStore.Application.Registrations.Interfaces;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken);
}