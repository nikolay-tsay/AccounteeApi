namespace AccounteeDomain.Models;

public class UserIncomeDto
{
    public int? Id { get; set; }
    public int? IdUser { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Amount { get; set; }
}