using BookStore.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
