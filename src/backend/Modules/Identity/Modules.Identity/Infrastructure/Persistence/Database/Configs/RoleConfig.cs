using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain;

namespace Modules.Identity.Infrastructure.Persistence.Database.Configs;

public sealed class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity("RolePermissions");
    }
}