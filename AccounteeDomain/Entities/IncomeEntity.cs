using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class IncomeEntity : IBaseWithCompany, ISearchable
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public int IdCategory { get; set; }
    public int? IdService { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(250)]
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime LastEdited { get; set; }
    public decimal TotalAmount { get; set; }
    public string SearchValue => Name.ToLower();
    
    public CompanyEntity? Company { get; set; }
    public ServiceEntity? Service { get; set; }
    public CategoryEntity IncomeCategory { get; set; } = null!;

    public IEnumerable<UserIncomeEntity>? UserIncomeList { get; set; }
    public IEnumerable<IncomeProductEntity>? IncomeProductList { get; set; }
    
}