using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Role;

public sealed class EditRoleHandler : IRequestHandler<EditRoleCommand, RoleResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditRoleHandler(IRoleRepository roleRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<RoleResponse> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditRoles, cancellationToken);

        var role = await _roleRepository.GetById(request.Id, true, false, cancellationToken);

        role!.Name = request.Name ?? role.Name;
        role.Description = request.Description ?? role.Description;
        role.IsAdmin = request.IsAdmin ?? role.IsAdmin;
        role.CanEditCompany = request.CanEditCompany ?? role.CanEditCompany;
        role.CanReadUsers = request.CanReadUsers ?? role.CanReadUsers;
        role.CanRegisterUsers = request.CanRegisterUsers ?? role.CanRegisterUsers;
        role.CanEditUsers = request.CanEditUsers ?? role.CanEditUsers;
        role.CanDeleteUsers = request.CanDeleteUsers ?? role.CanDeleteUsers;
        role.CanReadRoles = request.CanReadRoles ?? role.CanReadRoles;
        role.CanCreateRoles = request.CanCreateRoles ?? role.CanCreateRoles;
        role.CanEditRoles = request.CanEditRoles ?? role.CanEditRoles;
        role.CanReadOutlay = request.CanReadOutlay ?? role.CanReadOutlay;
        role.CanCreateOutlay = request.CanCreateOutlay ?? role.CanCreateOutlay;
        role.CanEditOutlay = request.CanEditOutlay ?? role.CanEditOutlay;
        role.CanDeleteOutlay = request.CanDeleteOutlay ?? role.CanDeleteOutlay;
        role.CanReadProducts = request.CanReadProducts ?? role.CanReadProducts;
        role.CanCreateProducts = request.CanCreateProducts ?? role.CanCreateProducts;
        role.CanEditProducts = request.CanEditProducts ?? role.CanEditProducts;
        role.CanDeleteProducts = request.CanDeleteProducts ?? role.CanDeleteProducts;
        role.CanReadServices = request.CanReadServices ?? role.CanReadServices;
        role.CanCreateServices = request.CanCreateServices ?? role.CanCreateServices;
        role.CanEditServices = request.CanEditServices ?? role.CanEditServices;
        role.CanDeleteServices = request.CanDeleteServices ?? role.CanDeleteServices;
        role.CanReadCategories = request.CanReadCategories ?? role.CanReadCategories;
        role.CanCreateCategories = request.CanCreateCategories ?? role.CanCreateCategories;
        role.CanEditCategories = request.CanEditCategories ?? role.CanEditCategories;
        role.CanDeleteCategories = request.CanDeleteCategories ?? role.CanDeleteCategories;
        role.CanUploadFiles = request.CanUploadFiles ?? role.CanUploadFiles;

        await _roleRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<RoleResponse>(role);
        return mapped;
    }
}