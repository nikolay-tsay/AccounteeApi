using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;

namespace AccounteeDomain.Entities.Relational;

public class OutcomeProductEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public int IdProduct { get; set; }
    public int IdOutcome { get; set; }

    public decimal Amount { get; set; }
    
    public CompanyEntity? Company { get; set; }
    public ProductEntity Product { get; set; } = null!;
    public OutcomeEntity Outcome { get; set; } = null!;
}