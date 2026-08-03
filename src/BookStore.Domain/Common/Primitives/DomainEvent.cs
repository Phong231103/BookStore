using BookStore.Domain.Common.Intefaces;

namespace BookStore.Domain.Common.Primitives
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        /// <inheritdoc />
        public Guid EventId { get; }

        /// <inheritdoc />
        public DateTime OccurredOnUtc { get; }
    }
}
