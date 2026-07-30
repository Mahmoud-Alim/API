using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public new DbSet<User> Users { get; set; } = null!;
    public DbSet<UserJobInfo> UserJobInfos { get; set; } = null!;
    public DbSet<UserSalary> UserSalaries { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.UserId).ValueGeneratedOnAdd();
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Gender).IsRequired().HasMaxLength(20);
            entity.Property(u => u.Active).IsRequired().HasDefaultValue(true);
        });

        modelBuilder.Entity<UserJobInfo>(entity =>
        {
            entity.ToTable("UserJobInfos");
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.JobTitle).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Department).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UserSalary>(entity =>
        {
            entity.ToTable("UserSalaries");
            entity.HasKey(x => x.UserId);
            entity.Property(x => x.Salary).HasColumnType("decimal(18,2)").IsRequired();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.TokenHash).IsRequired().HasMaxLength(64);
            entity.HasIndex(rt => rt.TokenHash).IsUnique();
            entity.Property(rt => rt.UserId).IsRequired();
            entity.Property(rt => rt.ExpirationDate).IsRequired();
            entity.Property(rt => rt.CreatedDate).IsRequired();
            entity.Property(rt => rt.CreatedByIp).IsRequired().HasMaxLength(45);
            entity.Property(rt => rt.RevokedByIp).HasMaxLength(45);
            entity.Property(rt => rt.ReplacedByTokenHash).HasMaxLength(64);
            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(al => al.Id);
            entity.Property(al => al.UserId).IsRequired();
            entity.Property(al => al.Action).IsRequired().HasMaxLength(100);
            entity.Property(al => al.EntityName).IsRequired().HasMaxLength(100);
            entity.Property(al => al.EntityId).HasMaxLength(100);
            entity.Property(al => al.OldValues).HasMaxLength(2000);
            entity.Property(al => al.NewValues).HasMaxLength(2000);
            entity.Property(al => al.CreatedDate).IsRequired();
            entity.Property(al => al.IpAddress).IsRequired().HasMaxLength(45);
            entity.HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}