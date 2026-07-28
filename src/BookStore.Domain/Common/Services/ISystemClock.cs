namespace BookStore.Domain.Common.Services;

/// <summary>
/// Provides the current UTC time.
/// </summary>
public interface ISystemClock
{
    DateTime UtcNow { get; }
}