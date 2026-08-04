using BookStore.Domain.Common.Primitives;
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
            //if (string.IsNullOrWhiteSpace(phoneNumber))
            //    throw new InvalidPhoneNumberException();

            var normalized = Normalize(phoneNumber);

            if (!PhoneRegex.IsMatch(normalized))
                throw new InvalidPhoneNumberException();

            return new PhoneNumber(normalized);
        }

        private static string Normalize(string value)
        {
            value = value.Trim();

            value = value.Replace(" ", "")
                         .Replace("-", "")
                         .Replace("(", "")
                         .Replace(")", "");

            return value;
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
