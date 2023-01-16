using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;

namespace AccounteeDomain.Entities.Relational;

public sealed class UserServiceEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    
    public int IdUser { get; set; }
    public int IdService { get; set; }

    public CompanyEntity? Company { get; set; }
    public UserEntity User { get; set; } = null!;
    public ServiceEntity Service { get; set; } = null!;
}