﻿using Domain.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(DatabaseSchema.UsersTable);

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(DatabaseSchema.UserFirstNameMaxLength);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(DatabaseSchema.UserLastNameMaxLength);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(DatabaseSchema.UserEmailMaxLength);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.Gender)
                   .IsRequired()
                   .HasMaxLength(DatabaseSchema.UserGenderMaxLength);

            builder.Property(u => u.Active)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}
