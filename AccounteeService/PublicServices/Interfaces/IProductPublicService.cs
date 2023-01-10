using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;

namespace AccounteeService.PublicServices.Interfaces;

public interface IProductPublicService
{
    Task<PagedList<ProductDto>> GetProducts(string? searchValue, OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken);
    Task<ProductDto> GetProductById(int productId, CancellationToken cancellationToken);
    Task<ProductDto> CreateProduct(ProductDto model, CancellationToken cancellationToken);
    Task<ProductDto> EditProduct(int productId, ProductDto model, CancellationToken cancellationToken);
    Task<ProductDto> ChangeProductCategory(int productId, int categoryId, CancellationToken cancellationToken);
    Task<bool> DeleteProduct(int productId, CancellationToken cancellationToken);
}