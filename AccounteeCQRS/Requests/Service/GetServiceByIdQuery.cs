using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record GetServiceByIdQuery(int Id) : IRequest<ServiceResponse>;