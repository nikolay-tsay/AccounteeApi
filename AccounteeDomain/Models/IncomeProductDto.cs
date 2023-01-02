namespace AccounteeDomain.Models;

public class IncomeProductDto
{
    public int? Id { get; set; }
    public int? IdProduct { get; set; }
    public string? ProductName { get; set; }
    public decimal Amount { get; set; }
}