using AccounteeDomain.Entities.Enums;
using MediatR;

namespace AccounteeCQRS.Requests.Category;

public record DeleteCategoryCommand(int Id, CategoryTargets Target) : IRequest<bool>;