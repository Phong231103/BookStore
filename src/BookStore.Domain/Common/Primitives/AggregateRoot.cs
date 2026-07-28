using BookStore.Domain.Common.Intefaces;

namespace BookStore.Domain.Common.Primitives;

/// <summary>
/// Represents the base class for all aggregate roots.
/// </summary>
public abstract class AggregateRoot<TId>
    : Entity<TId>,
      IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id)
        : base(id)
    {
    }

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    /// <summary>
    /// Records a new domain event.
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}