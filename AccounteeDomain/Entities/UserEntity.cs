using System.ComponentModel.DataAnnotations;

namespace AccounteeDomain.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }
    public int IdRole { get; set; }
    public int? IdCompany { get; set; }

    [MaxLength(25)]
    public string Login { get; set; } = null!;
    
    [MaxLength(50)]
    public string PasswordHash { get; set; } = null!;
    
    [MaxLength(50)]
    public string PasswordSalt { get; set; } = null!;
    
    [MaxLength(25)]
    public string FirstName { get; set; } = null!;
    
    [MaxLength(25)]
    public string LastName { get; set; } = null!;
    
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public CompanyEntity? Company { get; set; }
    public RoleEntity Role { get; set; } = null!;
}