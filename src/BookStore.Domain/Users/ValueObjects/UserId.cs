using BookStore.Domain.Users.Exceptions;

namespace BookStore.Domain.Users.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }

        public static UserId Create(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidUserIdException();

            return new UserId(value);
        }

        public static UserId New()
        {
            return new UserId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(UserId id) => id.Value;
    }
}
