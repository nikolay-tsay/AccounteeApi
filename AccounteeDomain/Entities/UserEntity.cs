using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Entities.Relational;

namespace AccounteeDomain.Entities;

public class UserEntity : IBaseWithCompany, ISearchable
{
    [Key]
    public int Id { get; set; }
    public int IdRole { get; set; }
    public int? IdCompany { get; set; }
    public UserLanguages UserLanguage { get; set; }

    [MaxLength(25)]
    public required string Login { get; set; }
    
    [MaxLength(50)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(50)]
    public required string PasswordSalt { get; set; }
    
    [MaxLength(25)]
    public required string FirstName { get; set; }
    
    [MaxLength(25)]
    public required string LastName { get; set; }
    
    [MaxLength(50)]
    public required string Email { get; set; }
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public decimal? IncomePercent { get; set; }
    public string SearchValue => $"{Login.ToLower()} {FirstName.ToLower()} {LastName.ToLower()} {Email.ToLower()}";

    public CompanyEntity? Company { get; set; }
    public RoleEntity Role { get; set; } = null!;
    public IEnumerable<UserIncomeEntity>? UserIncomeList { get; set; }
    public IEnumerable<UserServiceEntity>? UserServiceList { get; set; }
}