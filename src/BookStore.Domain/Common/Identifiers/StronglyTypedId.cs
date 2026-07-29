namespace BookStore.Domain.Common.Identifiers
{
    public abstract class StronglyTypedId
    {
        protected StronglyTypedId(Guid value)
        {
            ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);

            Value = value;
        }

        /// <summary>
        /// Gets the underlying Guid value.
        /// </summary>
        public Guid Value { get; }

        public sealed override bool Equals(object? obj)
        {
            return obj is StronglyTypedId other
                   && GetType() == other.GetType()
                   && Value == other.Value;
        }

        public sealed override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Value);
        }

        public sealed override string ToString()
        {
            return Value.ToString();
        }
    }
}
