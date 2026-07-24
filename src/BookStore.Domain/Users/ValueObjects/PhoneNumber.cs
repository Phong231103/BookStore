using BookStore.Domain.Users.Exceptions;
using System.Text.RegularExpressions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class PhoneNumber : ValueObject
    {
        private static readonly Regex PhoneRegex =
            new(@"^\+?[0-9]{8,15}$", RegexOptions.Compiled);

        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber);

            var normalized = phoneNumber.Trim();

            if (!PhoneRegex.IsMatch(normalized))
                throw new InvalidPhoneNumberException(phoneNumber);

            return new PhoneNumber(normalized);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
