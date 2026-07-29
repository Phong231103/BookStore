using BookStore.Domain.Common.Primitives;
using BookStore.Domain.Users.Exceptions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get; }

        private PasswordHash(string value)
        {
            Value = value;
        }

        public static PasswordHash Create(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new InvalidPasswordException();

            return new PasswordHash(hash);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
            => Value;
    }
}
