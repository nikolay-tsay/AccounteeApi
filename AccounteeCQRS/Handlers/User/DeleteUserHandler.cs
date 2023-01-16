using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.User;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanDeleteUsers);

        if (request.Id == currentUser.User.Id)
        {
            throw new AccounteeBadOperationException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.CannotDeleteSelf)));
        }

        var user = await _userRepository.GetById(request.Id, true, false, cancellationToken);
        await _userRepository.DeleteUser(user!, true, cancellationToken);

        return true;
    }
}