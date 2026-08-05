using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(DatabaseSchema.AspNetUsersTable);

        builder.Property(u => u.FirstName)
               .HasMaxLength(DatabaseSchema.UserFirstNameMaxLength);

        builder.Property(u => u.LastName)
               .HasMaxLength(DatabaseSchema.UserLastNameMaxLength);

        builder.Property(u => u.CreatedAt)
               .IsRequired();
    }
}
