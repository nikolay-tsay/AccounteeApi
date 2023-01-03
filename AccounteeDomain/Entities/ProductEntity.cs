using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class ProductEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }

    public int? IdCategory { get; set; }
    public int? IdCompany { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(250)]
    public string? Description { get; set; }
    public MeasurementUnits AmountUnit { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalPrice { get; set; }

    public CategoryEntity ProductCategory { get; set; } = null!;
    public CompanyEntity? Company { get; set; }

    public IEnumerable<IncomeProductEntity>? IncomeProductList { get; set; }
    public IEnumerable<ServiceProductEntity>? ServiceProductList { get; set; }
    public IEnumerable<OutcomeProductEntity>? OutcomeProductList { get; set; }
}