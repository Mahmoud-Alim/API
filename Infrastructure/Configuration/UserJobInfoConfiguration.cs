﻿using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;


public class UserJobInfoConfiguration : IEntityTypeConfiguration<UserJobInfo>
{
    public void Configure(EntityTypeBuilder<UserJobInfo> builder)
    {
        builder.ToTable(DatabaseSchema.UserJobInfosTable);

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.JobTitle)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.JobTitleMaxLength);

        builder.Property(x => x.Department)
               .IsRequired()
               .HasMaxLength(DatabaseSchema.DepartmentMaxLength);

    }
}
