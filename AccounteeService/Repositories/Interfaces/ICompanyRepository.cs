using AccounteeDomain.Entities;

namespace AccounteeService.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task AddCompany(CompanyEntity toAdd, bool save, CancellationToken cancellationToken);
    Task DeleteCompany(CompanyEntity toDelete, bool save, CancellationToken cancellationToken);
    Task SaveChanges(CancellationToken cancellationToken);
}