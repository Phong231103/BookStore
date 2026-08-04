using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Registrations.Models
{
    public sealed record EmailMessage
    {
        public required Email To { get; init; }

        public required string Subject { get; init; }

        public required string Body { get; init; }
    }
}
