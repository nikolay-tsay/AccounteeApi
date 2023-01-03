using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class CompanyEntity
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(30)]
    public string Name { get; set; } = null!;
    
    [MaxLength(50)]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public decimal Budget { get; set; }

    public IEnumerable<UserEntity> UserList { get; set; } = null!;
    public IEnumerable<RoleEntity>? RoleList { get; set; }
    public IEnumerable<IncomeEntity>? IncomeList { get; set; }
    public IEnumerable<OutcomeEntity>? OutcomeList { get; set; }
    public IEnumerable<ProductEntity>? ProductList { get; set; }
    public IEnumerable<ServiceEntity>? ServiceList { get; set; }
    public IEnumerable<CategoryEntity>? CategoryList { get; set; }
    public IEnumerable<IncomeProductEntity>? IncomeProductList { get; set; }
    public IEnumerable<OutcomeProductEntity>? OutcomeProductList { get; set; }
    public IEnumerable<ServiceProductEntity>? ServiceProductList { get; set; }
    public IEnumerable<UserIncomeEntity>? UserIncomeList { get; set; }
    public IEnumerable<UserServiceEntity>? UserServiceList { get; set; }
}