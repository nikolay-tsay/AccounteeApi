using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class ServiceEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }

    public int? IdCompany { get; set; }
    public int IdCategory { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(250)]
    public string? Description { get; set; }
    public decimal TotalPrice { get; set; }
    
    public CompanyEntity? Company { get; set; }
    public CategoryEntity ServiceCategory { get; set; } = null!;
    public IEnumerable<UserServiceEntity>? UserServiceList { get; set; }
    public IEnumerable<ServiceProductEntity>? ServiceProductList { get; set; }
    public IEnumerable<IncomeEntity>? IncomeList { get; set; }
}