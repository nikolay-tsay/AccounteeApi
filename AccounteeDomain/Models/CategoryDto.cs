namespace AccounteeDomain.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public int? IdCompany { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
}