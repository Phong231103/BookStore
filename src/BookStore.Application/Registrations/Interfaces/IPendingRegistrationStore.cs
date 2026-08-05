using BookStore.Application.Registration.Common;
using BookStore.Domain.Users.ValueObjects;

namespace BookStore.Application.Registration.Interfaces
{
    public interface IPendingRegistrationStore
    {
        Task SaveAsync(PendingRegistration registration, CancellationToken cancellationToken);

        Task<PendingRegistration?> GetAsync(Guid registrationId, CancellationToken cancellationToken);

        Task RemoveAsync(Guid registrationId, CancellationToken cancellationToken);

        Task<PendingRegistration?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    }
}
