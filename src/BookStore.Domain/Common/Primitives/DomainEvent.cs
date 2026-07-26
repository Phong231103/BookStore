using BookStore.Domain.Common.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
