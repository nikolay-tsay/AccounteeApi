using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;

namespace AccounteeDomain.Entities.Relational;

public class UserIncomeEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public int IdUser { get; set; }
    public int IdIncome { get; set; }
    
    public decimal Amount { get; set; }

    public CompanyEntity? Company { get; set; }
    public UserEntity User { get; set; } = null!;
    public IncomeEntity Income { get; set; } = null!;
}