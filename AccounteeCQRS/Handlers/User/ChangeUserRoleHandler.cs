using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ChangeUserRoleHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    
    public async Task<UserResponse> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditUsers, cancellationToken);
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditRoles, cancellationToken);

        var user = await _userRepository.GetById(request.UserId, true, false, cancellationToken);
        var role = await _roleRepository.GetById(request.RoleId, true, false, cancellationToken);

        user!.Role = role!;
        await _userRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<UserResponse>(user);
        return mapped;
    }
}