using AccounteeCQRS.Responses;
using MediatR;

namespace AccounteeCQRS.Requests.Role;

public record CreateRoleCommand : IRequest<RoleResponse>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    
    public bool? IsAdmin { get; init; }
    
    public bool? CanCreateCompany { get; init; }
    public bool? CanEditCompany { get; init; }
    public bool? CanDeleteCompany { get; init; }
    
    public bool? CanReadUsers { get; init; }
    public bool? CanRegisterUsers { get; init; }
    public bool? CanEditUsers { get; init; }
    public bool? CanDeleteUsers { get; init; }
    
    public bool? CanReadRoles { get; init; }
    public bool? CanCreateRoles { get; init; }
    public bool? CanEditRoles { get; init; }
    public bool? CanDeleteRoles { get; init; }
    
    public bool? CanReadOutlay { get; init; }
    public bool? CanCreateOutlay { get; init; }
    public bool? CanEditOutlay { get; init; }
    public bool? CanDeleteOutlay { get; init; }
    
    public bool? CanReadProducts { get; init; }
    public bool? CanCreateProducts { get; init; }
    public bool? CanEditProducts { get; init; }
    public bool? CanDeleteProducts { get; init; }
    
    public bool? CanReadServices { get; init; }
    public bool? CanCreateServices { get; init; }
    public bool? CanEditServices { get; init; }
    public bool? CanDeleteServices { get; init; }
    
    public bool? CanReadCategories { get; init; }
    public bool? CanCreateCategories { get; init; }
    public bool? CanEditCategories { get; init; }
    public bool? CanDeleteCategories { get; init; }
    
    public bool? CanUploadFiles { get; init; }
}