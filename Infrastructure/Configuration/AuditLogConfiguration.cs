using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(al => al.Id);

        builder.Property(al => al.UserId)
               .IsRequired();

        builder.Property(al => al.Action)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(al => al.EntityName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(al => al.EntityId)
               .HasMaxLength(100);

        builder.Property(al => al.OldValues)
               .HasMaxLength(2000);

        builder.Property(al => al.NewValues)
               .HasMaxLength(2000);

        builder.Property(al => al.CreatedDate)
               .IsRequired();

        builder.Property(al => al.IpAddress)
               .IsRequired()
               .HasMaxLength(45);

        builder.HasOne(al => al.User)
               .WithMany()
               .HasForeignKey(al => al.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}