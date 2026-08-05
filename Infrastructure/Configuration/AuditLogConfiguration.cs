using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(DatabaseSchema.AuditLogsTable);

        builder.HasKey(al => al.Id);

        builder.Property(al => al.UserId)
               .IsRequired();

        builder.Property(al => al.Action)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.AuditActionMaxLength);

        builder.Property(al => al.EntityName)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.AuditEntityNameMaxLength);

        builder.Property(al => al.EntityId)
               .HasMaxLength(DatabaseSchema.AuditEntityIdMaxLength);

        builder.Property(al => al.OldValues)
               .HasMaxLength(DatabaseSchema.AuditValuesMaxLength);

        builder.Property(al => al.NewValues)
               .HasMaxLength(DatabaseSchema.AuditValuesMaxLength);

        builder.Property(al => al.CreatedDate)
               .IsRequired();

        builder.Property(al => al.IpAddress)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.IpAddressMaxLength);

        builder.HasOne(al => al.User)
               .WithMany()
               .HasForeignKey(al => al.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
