namespace AccounteeDomain.Entities.Base;

public interface IBaseWithCompany
{
    public int? IdCompany { get; set; }
    public CompanyEntity? Company { get; set; }
}