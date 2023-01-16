using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class ProductEntity : IBaseWithCompany, ISearchable
{
    [Key]
    public int Id { get; set; }

    public int IdCategory { get; set; }
    public int? IdCompany { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(250)]
    public string? Description { get; set; }
    public required MeasurementUnits AmountUnit { get; set; }
    public required decimal Amount { get; set; }
    public required decimal TotalPrice { get; set; }
    public string SearchValue => Name.ToLower();

    public CategoryEntity ProductCategory { get; set; } = null!;
    public CompanyEntity? Company { get; set; }

    public IEnumerable<IncomeProductEntity>? IncomeProductList { get; set; }
    public IEnumerable<ServiceProductEntity>? ServiceProductList { get; set; }
    public IEnumerable<OutcomeProductEntity>? OutcomeProductList { get; set; }
}