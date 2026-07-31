using BookStore.Domain.Users.Identifiers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookStore.Infrastructure.Persistence.Converters;

public sealed class RoleIdConverter : ValueConverter<RoleId, Guid>
{
    public RoleIdConverter()
        : base(
            id => id.Value,
            value => RoleId.Create(value))
    {
    }
}
