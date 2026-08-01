using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;


public class UserJobInfoConfiguration : IEntityTypeConfiguration<UserJobInfo>
{
    public void Configure(EntityTypeBuilder<UserJobInfo> builder)
    {
        builder.ToTable("UserJobInfos");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.JobTitle)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Department)
               .IsRequired()
               .HasMaxLength(100);

    }
}
