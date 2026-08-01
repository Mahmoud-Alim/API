using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserSalaryConfiguration : IEntityTypeConfiguration<UserSalary>
{
    public void Configure(EntityTypeBuilder<UserSalary> builder)
    {
        builder.ToTable("UserSalaries");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Salary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

    }
}
