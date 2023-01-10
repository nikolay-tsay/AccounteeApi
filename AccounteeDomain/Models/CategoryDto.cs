using System.Text.Json.Serialization;
using AccounteeDomain.Entities.Enums;

namespace AccounteeDomain.Models;

public class CategoryDto
{
    public int Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CategoryTargets Target { get; set; }
    public int? IdCompany { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
}