using MediatR;

namespace BookStore.Application.Registrations.StartRegistration
{
    public sealed record StartRegistrationCommand
    : IRequest<StartRegistrationResponse>
    {
        public required string Email { get; init; }

        public required string Password { get; init; }

        public required string ConfirmPassword { get; init; }

        public required string FullName { get; init; }

        public string? PhoneNumber { get; init; }
    }
}
