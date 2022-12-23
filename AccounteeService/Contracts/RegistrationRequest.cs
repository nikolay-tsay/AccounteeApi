namespace AccounteeService.Contracts;

public class RegistrationRequest
{
    public int? IdCompany { get; set; }
    public int IdRole { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}