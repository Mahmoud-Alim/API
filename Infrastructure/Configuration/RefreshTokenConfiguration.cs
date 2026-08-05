using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(DatabaseSchema.RefreshTokensTable);

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.TokenHash)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.TokenHashMaxLength);

        builder.HasIndex(rt => rt.TokenHash)
               .IsUnique();

        builder.Property(rt => rt.UserId)
               .IsRequired();

        builder.Property(rt => rt.ExpirationDate)
               .IsRequired();

        builder.Property(rt => rt.CreatedDate)
               .IsRequired();

        builder.Property(rt => rt.CreatedByIp)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.IpAddressMaxLength);

        builder.Property(rt => rt.RevokedByIp)
               .HasMaxLength(DatabaseSchema.IpAddressMaxLength);

        builder.Property(rt => rt.ReplacedByTokenHash)
               .HasMaxLength(DatabaseSchema.TokenHashMaxLength);

        builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
