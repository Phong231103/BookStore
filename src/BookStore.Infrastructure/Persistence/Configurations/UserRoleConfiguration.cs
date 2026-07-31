using BookStore.Domain.Users.ChildEntity;
using BookStore.Domain.Users.Identifiers;
using BookStore.Infrastructure.Persistence.Configurations.Base;
using BookStore.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : EntityTypeConfigurationBase<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserRoles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<RoleIdConverter>()
            .HasColumnName("RoleId")
            .ValueGeneratedNever();

        builder.Property<UserId>("UserId")
            .HasConversion<UserIdConverter>()
            .IsRequired();

        builder.Property(x => x.AssignedAt)
            .IsRequired();
    }
}
