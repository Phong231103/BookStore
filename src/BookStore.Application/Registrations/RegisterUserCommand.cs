using MediatR;

namespace BookStore.Application.Registration
{
    public sealed record RegisterUserCommand(string Email, string Password, string ConfirmPassword, string FullName, string? PhoneNumber) : IRequest<RegisterUserResponse>;
}
