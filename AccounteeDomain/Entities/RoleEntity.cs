using System.ComponentModel.DataAnnotations;

namespace AccounteeDomain.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(250)]
    public string? Description { get; set; }

    public bool IsAdmin { get; set; }
    public bool CanCreateCompany { get; set; }
    public bool CanRead { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanUploadFiles { get; set; }

    public CompanyEntity? Company { get; set; }
    public IEnumerable<UserEntity>? UserList { get; set; }
}