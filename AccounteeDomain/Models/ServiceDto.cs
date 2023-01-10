namespace AccounteeDomain.Models;

public class ServiceDto
{
    public int? Id { get; set; }
    public int? IdCategory { get; set; }
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public decimal? TotalPrice { get; set; }
}