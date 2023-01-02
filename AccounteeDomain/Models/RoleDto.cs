namespace AccounteeDomain.Models;

public class RoleDto
{
    public int? Id { get; set; }
    public int? IdCompany { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public bool? IsAdmin { get; set; }
    
    public bool? CanCreateCompany { get; set; }
    public bool? CanEditCompany { get; set; }
    public bool? CanDeleteCompany { get; set; }
    
    public bool? CanReadUsers { get; set; }
    public bool? CanRegisterUsers { get; set; }
    public bool? CanEditUsers { get; set; }
    public bool? CanDeleteUsers { get; set; }
    
    public bool? CanReadRoles { get; set; }
    public bool? CanCreateRoles { get; set; }
    public bool? CanEditRoles { get; set; }
    public bool? CanDeleteRoles { get; set; }
    
    public bool? CanReadOutlay { get; set; }
    public bool? CanCreateOutlay { get; set; }
    public bool? CanEditOutlay { get; set; }
    public bool? CanDeleteOutlay { get; set; }
}