using BookStore.Domain.Users.ValueObjects;
using FluentValidation;

namespace BookStore.Application.Registrations.StartRegistration;

public sealed class StartRegistrationCommandValidator
    : AbstractValidator<StartRegistrationCommand>
{
    public StartRegistrationCommandValidator()
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
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(FullName.MaxLength);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}