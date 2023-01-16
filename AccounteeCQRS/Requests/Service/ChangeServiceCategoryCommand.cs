using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Service;

public record ChangeServiceCategoryCommand(int ServiceId, int CategoryId) : IRequest<ServiceResponse>;