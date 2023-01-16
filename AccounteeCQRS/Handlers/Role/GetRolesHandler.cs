using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Role;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Role;

public sealed class GetRolesHandler : IRequestHandler<GetRolesQuery, PagedList<RoleResponse>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetRolesHandler(IMapper mapper, IRoleRepository roleRepository, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<PagedList<RoleResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadRoles, cancellationToken);

        var roles = await _roleRepository
            .QueryAll(false)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedList(request.PageFilter, cancellationToken);
        
        var result = _mapper.Map<PagedList<RoleResponse>>(roles);
        return result;
    }
}