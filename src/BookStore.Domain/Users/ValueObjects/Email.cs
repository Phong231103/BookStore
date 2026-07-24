using BookStore.Domain.Users.Exceptions;
using System.Text.RegularExpressions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex =
            new(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidEmailException(email);

            var normalized = email.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(normalized))
                throw new InvalidEmailException(email);

            return new Email(normalized);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
