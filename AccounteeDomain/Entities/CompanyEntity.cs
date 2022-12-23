using System.ComponentModel.DataAnnotations;

namespace AccounteeDomain.Entities;

public class CompanyEntity
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(30)]
    public string Name { get; set; } = null!;
    
    [MaxLength(50)]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public IEnumerable<RoleEntity>? RoleList { get; set; }
    public IEnumerable<UserEntity> UserList { get; set; } = null!;
}