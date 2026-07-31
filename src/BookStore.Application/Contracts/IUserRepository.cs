using BookStore.Domain.Users;
using BookStore.Domain.Users.Identifiers;

namespace BookStore.Application.Contracts;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    void Add(User user);

    void Remove(User user);
}
