namespace AccounteeDomain.Models;

public class UserDto
{
    public int? Id { get; set; }
    public int? IdCompany { get; set; }
    public int? IdRole { get; set; }
    public string? Login { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    
    public decimal? IncomePercent { get; set; }
}