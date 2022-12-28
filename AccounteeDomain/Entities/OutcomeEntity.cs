using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Categories;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class OutcomeEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public int IdCategory { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime LastEdited { get; set; }
    public decimal TotalPrice { get; set; }

    public CompanyEntity? Company { get; set; }
    public OutcomeCategoryEntity OutcomeCategory { get; set; } = null!;

    public IEnumerable<OutcomeProductEntity>? OutcomeProductList { get; set; }
}