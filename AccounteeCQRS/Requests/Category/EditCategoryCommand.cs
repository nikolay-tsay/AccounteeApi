using System.Text.Json.Serialization;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using MediatR;

namespace AccounteeCQRS.Requests.Category;

public record EditCategoryCommand : IRequest<CategoryResponse>
{
    public int Id { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required CategoryTargets Target { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}