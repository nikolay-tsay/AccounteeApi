using System.ComponentModel.DataAnnotations;
using AccounteeDomain.Entities.Base;

namespace AccounteeDomain.Entities.Relational;

public class ServiceProductEntity : IBaseWithCompany
{
    [Key]
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    
    public int IdProduct { get; set; }
    public int IdService { get; set; }

    public decimal? ProductUsedAmount { get; set; }
    public decimal? PricePerService { get; set; }

    public CompanyEntity? Company { get; set; }
    public ProductEntity Product { get; set; } = null!;
    public ServiceEntity Service { get; set; } = null!;
}