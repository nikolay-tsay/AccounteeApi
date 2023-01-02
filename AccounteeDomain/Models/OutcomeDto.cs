namespace AccounteeDomain.Models;

public class OutcomeDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? LastEdited { get; set; }
    public decimal? TotalPrice { get; set; }
}