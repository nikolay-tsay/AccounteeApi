using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Entities.Relational;
using AccounteeDomain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AccounteeDomain.Contexts;

public class AccounteeContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<CompanyEntity> Companies { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;
    public DbSet<IncomeEntity> Incomes { get; set; } = null!;
    public DbSet<OutcomeEntity> Outcomes { get; set; } = null!;
    public DbSet<ServiceEntity> Services { get; set; } = null!;
    public DbSet<ProductEntity> Products { get; set; } = null!;
    public DbSet<ServiceProductEntity> ServiceProducts { get; set; } = null!;
    public DbSet<UserIncomeEntity> UserIncomes { get; set; } = null!;
    public DbSet<UserServiceEntity> UserServices { get; set; } = null!;
    public DbSet<IncomeProductEntity> IncomeProducts { get; set; } = null!;
    public DbSet<OutcomeProductEntity> OutcomeProducts { get; set; } = null!;
    public DbSet<CategoryEntity> Categories { get; set; } = null!;

    public AccounteeContext(DbContextOptions<AccounteeContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>().CompanyFilter();
        modelBuilder.Entity<RoleEntity>().CompanyFilter();
        modelBuilder.Entity<IncomeEntity>().CompanyFilter();
        modelBuilder.Entity<OutcomeEntity>().CompanyFilter();
        modelBuilder.Entity<ServiceEntity>().CompanyFilter();
        modelBuilder.Entity<ProductEntity>().CompanyFilter();
        
        modelBuilder.Entity<ServiceProductEntity>().CompanyFilter();
        modelBuilder.Entity<UserIncomeEntity>().CompanyFilter();
        modelBuilder.Entity<UserServiceEntity>().CompanyFilter();
        modelBuilder.Entity<IncomeProductEntity>().CompanyFilter();
        modelBuilder.Entity<OutcomeProductEntity>().CompanyFilter();
        
        modelBuilder.Entity<CategoryEntity>().CompanyFilter();

        modelBuilder.Entity<IncomeEntity>().Property(x => x.DateTime).UtcDate();
        modelBuilder.Entity<IncomeEntity>().Property(x => x.LastEdited).UtcDate();
        
        modelBuilder.Entity<OutcomeEntity>().Property(x => x.DateTime).UtcDate();
        modelBuilder.Entity<OutcomeEntity>().Property(x => x.LastEdited).UtcDate();

        modelBuilder.HasPostgresEnum<MeasurementUnits>();
        modelBuilder.HasPostgresEnum<CategoryTargets>();
        
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

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<IncomeEntity>(x => x.IncomeList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<OutcomeEntity>(x => x.OutcomeList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CompanyEntity>()
            .HasMany<ProductEntity>(x => x.ProductList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CompanyEntity>()
            .HasMany<ServiceEntity>(x => x.ServiceList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<IncomeProductEntity>(x => x.IncomeProductList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompanyEntity>()
            .HasMany<OutcomeProductEntity>(x => x.OutcomeProductList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CompanyEntity>()
            .HasMany<ServiceProductEntity>(x => x.ServiceProductList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CompanyEntity>()
            .HasMany<UserIncomeEntity>(x => x.UserIncomeList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CompanyEntity>()
            .HasMany<UserServiceEntity>(x => x.UserServiceList)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.IdCompany)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasOne<RoleEntity>(x => x.Role)
            .WithMany(x => x.UserList)
            .HasForeignKey(x => x.IdRole)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserEntity>()
            .HasMany<UserIncomeEntity>(x => x.UserIncomeList)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserEntity>()
            .HasMany<UserServiceEntity>(x => x.UserServiceList)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.IdUser)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IncomeEntity>()
            .HasMany<UserIncomeEntity>(x => x.UserIncomeList)
            .WithOne(x => x.Income)
            .HasForeignKey(x => x.IdIncome)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IncomeEntity>()
            .HasMany<IncomeProductEntity>(x => x.IncomeProductList)
            .WithOne(x => x.Income)
            .HasForeignKey(x => x.IdIncome)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OutcomeEntity>()
            .HasMany<OutcomeProductEntity>(x => x.OutcomeProductList)
            .WithOne(x => x.Outcome)
            .HasForeignKey(x => x.IdOutcome)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductEntity>()
            .HasMany<IncomeProductEntity>(x => x.IncomeProductList)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductEntity>()
            .HasMany<ServiceProductEntity>(x => x.ServiceProductList)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductEntity>()
            .HasMany<OutcomeProductEntity>(x => x.OutcomeProductList)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.IdProduct)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceEntity>()
            .HasMany<UserServiceEntity>(x => x.UserServiceList)
            .WithOne(x => x.Service)
            .HasForeignKey(x => x.IdService)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ServiceEntity>()
            .HasMany<ServiceProductEntity>(x => x.ServiceProductList)
            .WithOne(x => x.Service)
            .HasForeignKey(x => x.IdService)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ServiceEntity>()
            .HasMany<IncomeEntity>(x => x.IncomeList)
            .WithOne(x => x.Service)
            .HasForeignKey(x => x.IdService)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryEntity>()
            .HasMany<IncomeEntity>(x => x.IncomeList)
            .WithOne(x => x.IncomeCategory)
            .HasForeignKey(x => x.IdCategory)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CategoryEntity>()
            .HasMany<OutcomeEntity>(x => x.OutcomeList)
            .WithOne(x => x.OutcomeCategory)
            .HasForeignKey(x => x.IdCategory)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CategoryEntity>()
            .HasMany<ProductEntity>(x => x.ProductList)
            .WithOne(x => x.ProductCategory)
            .HasForeignKey(x => x.IdCategory)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CategoryEntity>()
            .HasMany<ServiceEntity>(x => x.ServiceList)
            .WithOne(x => x.ServiceCategory)
            .HasForeignKey(x => x.IdCategory)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CategoryEntity>()
            .HasIndex(x => new { x.IdCompany ,x.Name, x.Target})
            .IsUnique();

        modelBuilder.Entity<ServiceEntity>()
            .HasIndex(x => new { x.IdCompany, x.Name })
            .IsUnique();
        
        modelBuilder.Entity<ProductEntity>()
            .HasIndex(x => new { x.IdCompany, x.Name })
            .IsUnique();
        
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