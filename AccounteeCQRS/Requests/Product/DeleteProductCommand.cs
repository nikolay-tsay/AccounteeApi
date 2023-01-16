using MediatR;

namespace AccounteeCQRS.Requests.Product;

public record DeleteProductCommand(int Id) : IRequest<bool>;