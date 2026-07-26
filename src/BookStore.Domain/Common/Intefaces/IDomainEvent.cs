using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Common.Intefaces
{
    public interface IDomainEvent
    {
        /// <summary>
        /// Unique identifier of the event.
        /// Used for idempotency when publishing integration events.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// UTC timestamp indicating when the event occurred.
        /// </summary>
        DateTime OccurredOnUtc { get; }
    }
}
