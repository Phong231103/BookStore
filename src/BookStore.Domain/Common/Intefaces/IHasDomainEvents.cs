namespace BookStore.Domain.Common.Intefaces;

/// <summary>
/// Represents an aggregate that records domain events.
/// </summary>
public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}