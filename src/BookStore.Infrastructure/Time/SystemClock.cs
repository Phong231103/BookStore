using BookStore.Domain.Common.Services;

namespace BookStore.Infrastructure.Common;

public sealed class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
