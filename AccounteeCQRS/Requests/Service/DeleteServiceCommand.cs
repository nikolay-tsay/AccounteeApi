using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record DeleteServiceCommand(int Id) : IRequest<bool>;