using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;

namespace AccounteeService.PublicServices.Interfaces;

public interface ICategoryPublicService
{
    Task<PagedList<CategoryDto>> GetCategories(PageFilter filter, CategoryTargets target, CancellationToken cancellationToken);
    Task<CategoryDto> CreateCategory(CategoryDto model, CancellationToken cancellationToken);
    Task<CategoryDto> EditCategory(int id, CategoryDto model, CategoryTargets target, CancellationToken cancellationToken);
    Task<bool> DeleteCategory(CategoryTargets target, int id, CancellationToken cancellationToken);
}