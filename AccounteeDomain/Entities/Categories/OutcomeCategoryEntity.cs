using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;

namespace AccounteeDomain.Entities.Categories;

public class OutcomeCategoryEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(250)]
    public string? Description { get; set; }
   
    public CompanyEntity? Company { get; set; }
    public IEnumerable<OutcomeEntity>? OutcomeList { get; set; }
}