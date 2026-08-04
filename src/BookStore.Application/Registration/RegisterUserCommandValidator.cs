using BookStore.Domain.Users.ValueObjects;
using FluentValidation;

namespace BookStore.Application.Registration;

public sealed class RegisterUserCommandValidator
    : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(Email.MaxLength);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(PasswordHash.MinLength)
            .MaximumLength(PasswordHash.MaxLength);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(FullName.MaxLength);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}