using AccounteeDomain.Entities.Enums;

namespace AccounteeDomain.Models;

public class ProductDto
{
    public int? Id { get; set; }
    public int? IdCategory { get; set; }
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public MeasurementUnits? AmountUnit { get; set; }
    public decimal? Amount { get; set; }
    public decimal? TotalPrice { get; set; }
}