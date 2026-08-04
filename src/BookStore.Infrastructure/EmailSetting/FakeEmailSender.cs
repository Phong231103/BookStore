using BookStore.Application.Registrations.Interfaces;
using BookStore.Application.Registrations.Models;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.EmailSetting;

internal sealed class FakeEmailSender : IEmailSender
{
    private readonly ILogger<FakeEmailSender> _logger;

    public FakeEmailSender(
        ILogger<FakeEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation(
            """
            ===== EMAIL =====
            To: {To}
            Subject: {Subject}

            {Body}
            =================
            """,
            message.To.Value,
            message.Subject,
            message.Body);

        return Task.CompletedTask;
    }
}