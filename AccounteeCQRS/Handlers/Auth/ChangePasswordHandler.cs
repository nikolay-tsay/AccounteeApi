using System.Globalization;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Auth;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Auth;

public sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;

    public ChangePasswordHandler(ICurrentUserService currentUserService, IPasswordHandler passwordHandler, IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _passwordHandler = passwordHandler;
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        if (userId is null)
        {
            userId = _currentUserService.GetCurrentUserId();
        }

        var user = await _userRepository.GetById(userId.Value, false, true, cancellationToken);

        var pwdValid = _passwordHandler.IsValid(request.OldPwd, user!.PasswordHash, user.PasswordSalt);
        if (!pwdValid)
        {
            throw new AccounteeUnauthorizedException(ResourceRetriever.Get(CultureInfo.CurrentCulture, 
                nameof(Resources.InvalidPassword), user.Login));
        }
        
        _passwordHandler.CreateHash(request.NewPwd, out string hash, out string salt);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        await _userRepository.SaveChanges(cancellationToken);
        return true;
    }
}