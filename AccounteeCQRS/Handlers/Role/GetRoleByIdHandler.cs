using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Role;

public sealed class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    
    public GetRoleByIdHandler(IRoleRepository roleRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<RoleResponse> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadRoles, cancellationToken);

        var role = await _roleRepository.GetById(request.Id, false, false, cancellationToken);

        var mapped = _mapper.Map<RoleResponse>(role);
        return mapped;
    }
}