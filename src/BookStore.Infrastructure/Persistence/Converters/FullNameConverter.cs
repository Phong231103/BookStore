using BookStore.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookStore.Infrastructure.Persistence.Converters;

public sealed class FullNameConverter : ValueConverter<FullName, string>
{
    public FullNameConverter()
        : base(
            name => name.Value,
            value => FullName.Create(value))
    {
    }
}
