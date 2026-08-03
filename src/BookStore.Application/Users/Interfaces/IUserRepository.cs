using BookStore.Domain.Users;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
    }
}
