using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record ChangeProductCategoryCommand(int ProductId, int CategoryId) : IRequest<ProductResponse>;