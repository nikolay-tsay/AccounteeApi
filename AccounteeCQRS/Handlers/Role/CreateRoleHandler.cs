using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Role;

public sealed class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateRoleHandler(IRoleRepository roleRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<RoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanCreateRoles);

        var existing = await _roleRepository.GetByName(request.Name, false, true, cancellationToken);
        if (existing is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.AlreadyExists), nameof(RoleEntity)));
        }
        
        var newRole = _mapper.Map<RoleEntity>(request);
        if (newRole is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.MappingError), new object[] {nameof(CreateRoleCommand), nameof(RoleEntity)}));
        }

        newRole.IdCompany = currentUser.User.IdCompany;
        await _roleRepository.AddRole(newRole, true, cancellationToken);

        var mapped = _mapper.Map<RoleResponse>(newRole);
        return mapped;
    }
}