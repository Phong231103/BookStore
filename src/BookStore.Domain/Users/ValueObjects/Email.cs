using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Exceptions;
using System.Text.RegularExpressions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const int MaxLength = 256;
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            if (email.Length > MaxLength)
                throw new InvalidEmailException();

            var normalized = Normalize(email);

            if (!EmailRegex.IsMatch(normalized))
                throw new InvalidEmailException();

            return new Email(normalized);
        }

        private static string Normalize(string email)
        {
            return email.Trim().ToLowerInvariant();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
