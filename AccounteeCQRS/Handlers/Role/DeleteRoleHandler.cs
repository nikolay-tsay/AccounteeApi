using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Role;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Role;

public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteRoleHandler(IRoleRepository roleRepository, ICurrentUserService currentUserService)
    {
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanDeleteCompany, cancellationToken);
        
        var role = await _roleRepository.GetById(request.Id, true, false, cancellationToken);
        await _roleRepository.DeleteRole(role!, true, cancellationToken);

        return true;
    }
}