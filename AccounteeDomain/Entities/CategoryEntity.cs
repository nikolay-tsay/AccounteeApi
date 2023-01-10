using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Enums;

namespace AccounteeDomain.Entities;

public class CategoryEntity : IBaseWithCompany, ISearchable
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public CategoryTargets Target { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(250)]
    public string? Description { get; set; }

    public string SearchValue => Name.ToLower();

    public CompanyEntity? Company { get; set; }
    public IEnumerable<IncomeEntity>? IncomeList { get; set; }
    public IEnumerable<OutcomeEntity>? OutcomeList { get; set; }
    public IEnumerable<ProductEntity>? ProductList { get; set; }
    public IEnumerable<ServiceEntity>? ServiceList { get; set; } }