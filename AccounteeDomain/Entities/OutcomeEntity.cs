using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class OutcomeEntity : IBaseWithCompany, ISearchable
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public int IdCategory { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime LastEdited { get; set; }
    public decimal TotalPrice { get; set; }
    public string SearchValue => Name.ToLower();

    public CompanyEntity? Company { get; set; }
    public CategoryEntity OutcomeCategory { get; set; } = null!;

    public IEnumerable<OutcomeProductEntity>? OutcomeProductList { get; set; }
}