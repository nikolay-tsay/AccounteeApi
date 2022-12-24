using AccounteeDomain.Models;

namespace AccounteeService.PublicServices.Interfaces;

public interface ICompanyPublicService
{
    Task<CompanyDto> GetCompany(CancellationToken cancellationToken);
    Task<CompanyDto> CreateCompany(CompanyDto model, CancellationToken cancellationToken);
    Task<bool> DeleteCompany(CancellationToken cancellationToken);
    Task<CompanyDto> EditCompany(CompanyDto model, CancellationToken cancellationToken);
}