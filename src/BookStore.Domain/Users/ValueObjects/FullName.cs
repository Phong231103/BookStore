using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Exceptions;
using System.Text.RegularExpressions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public const int MaxLength = 100;

        public string Value { get; }

        private FullName(string value)
        {
            Value = value;
        }

        public static FullName Create(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new InvalidFullNameException();

            // Loại bỏ khoảng trắng thừa
            var normalized = Normalize(fullName);

            if (normalized.Length > MaxLength)
                throw new InvalidFullNameException();

            return new FullName(normalized);
        }

        private static string Normalize(string value)
        {
            return Regex.Replace(value.Trim(), @"\s+", " ");
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
            => Value;
    }
}
