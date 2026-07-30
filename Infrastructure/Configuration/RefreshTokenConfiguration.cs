using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.TokenHash)
               .IsRequired()
               .HasMaxLength(64);

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
               .HasMaxLength(45);

        builder.Property(rt => rt.RevokedByIp)
               .HasMaxLength(45);

        builder.Property(rt => rt.ReplacedByTokenHash)
               .HasMaxLength(64);

        builder.HasOne(rt => rt.User)
               .WithMany()
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}