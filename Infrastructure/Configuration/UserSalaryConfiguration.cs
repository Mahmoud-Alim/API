using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserSalaryConfiguration : IEntityTypeConfiguration<UserSalary>
{
    public void Configure(EntityTypeBuilder<UserSalary> builder)
    {
        builder.ToTable(DatabaseSchema.UserSalariesTable);

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Salary)
               .HasColumnType(DatabaseSchema.Decimal18_2)
               .IsRequired();

    }
}
