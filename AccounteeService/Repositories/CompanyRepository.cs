using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;

namespace AccounteeService.Repositories;

public sealed class CompanyRepository : ICompanyRepository
{
    private readonly AccounteeContext _context;

    public CompanyRepository(AccounteeContext context)
    {
        _context = context;
    }
    
    public async Task AddCompany(CompanyEntity toAdd, bool save, CancellationToken cancellationToken)
    {
        if (save)
        {
            _context.Companies.Add(toAdd);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        await _context.Companies.AddAsync(toAdd, cancellationToken);
    }

    public async Task DeleteCompany(CompanyEntity toDelete, bool save, CancellationToken cancellationToken)
    {
        _context.Companies.Remove(toDelete);

        if (save)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}