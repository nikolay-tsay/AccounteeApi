namespace AccounteeDomain.Models;

public class RoleDto
{
    public int Id { get; set; }
    public int IdCompany { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsAdmin { get; set; }
    public bool CanRead { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanUploadFiles { get; set; }
}