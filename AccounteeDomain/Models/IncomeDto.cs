namespace AccounteeDomain.Models;

public class IncomeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ServiceName { get; set; }
    public string? CategoryName { get; set; } 
    public string? Description { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? LastEdited { get; set; }
    public decimal? TotalAmount { get; set; }
}