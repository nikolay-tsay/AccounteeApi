using AccounteeDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccounteeDomain.Contexts;

public class AccounteeContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<CompanyEntity> Companies { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;
    
    public AccounteeContext(DbContextOptions<AccounteeContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<RoleEntity>(x => x.RoleList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<UserEntity>(x => x.UserList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserEntity>()
            .HasOne<RoleEntity>(x => x.Role)
            .WithMany(x => x.UserList)
            .HasForeignKey(x => x.IdRole)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserEntity>()
            .HasIndex(x => new { x.IdCompany, x.Login })
            .IsUnique();

        modelBuilder.Entity<RoleEntity>()
            .HasIndex(x => new { x.IdCompany, x.Name })
            .IsUnique();

        modelBuilder.Entity<RoleEntity>()
            .HasData(new RoleEntity
            {
                Id = 1,
                IdCompany = null,
                Name = "Visitor",
                CanCreateCompany = true
            });
    }
}