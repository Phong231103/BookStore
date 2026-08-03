using BookStore.Application.Users.Interfaces;
using BookStore.Domain.Users;
using BookStore.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    //{
    //    return await _dbContext.Users
    //        .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    //}

    //public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    //{
    //    var emailVo = Email.Create(email);
    //    return await _dbContext.Users
    //        .FirstOrDefaultAsync(u => u.Email == emailVo, cancellationToken);
    //}

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    //public async Task RemoveAsync(User user, CancellationToken cancellationToken = default)
    //{
    //    _dbContext.Users.Remove(user);
    //    await _dbContext.SaveChangesAsync(cancellationToken);
    //}
}
