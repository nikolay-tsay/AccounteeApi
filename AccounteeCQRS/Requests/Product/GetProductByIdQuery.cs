using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record GetProductByIdQuery(int Id) : IRequest<ProductResponse>;