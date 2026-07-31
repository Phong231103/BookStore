using BookStore.Domain.Users;
using BookStore.Infrastructure.Persistence.Configurations.Base;
using BookStore.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : EntityTypeConfigurationBase<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UserIdConverter>()
            .ValueGeneratedNever();

        builder.Property(x => x.Email)
            .HasConversion<EmailConverter>()
            .HasColumnName("Email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.FullName)
            .HasConversion<FullNameConverter>()
            .HasColumnName("FullName")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasConversion<PhoneNumberConverter>()
            .HasColumnName("PhoneNumber")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasConversion<PasswordHashConverter>()
            .HasColumnName("PasswordHash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.TwoFactorMethod)
            .HasConversion<int>();

        builder.Property(x => x.EmailConfirmed)
            .IsRequired();

        builder.Property(x => x.FailedLoginAttempts)
            .IsRequired();

        builder.Property(x => x.LockoutEndUtc);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc)
            .IsRequired();

        builder.Ignore(x => x.IsTwoFactorEnabled);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Roles)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation(nameof(User.Roles))!
            .SetField("_roles");
    }
}
