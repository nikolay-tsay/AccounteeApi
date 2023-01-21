using System.Globalization;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Auth;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Auth;

public sealed class LoginHandler : IRequestHandler<LoginQuery, string>
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;

    public LoginHandler(IJwtService jwtService, IPasswordHandler passwordHandler, IUserRepository userRepository)
    {
        _jwtService = jwtService;
        _passwordHandler = passwordHandler;
        _userRepository = userRepository;
    }
    
    public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);

        var user = await _userRepository.GetByLogin(request.Login, false, false, cancellationToken);

        var pwdValid = _passwordHandler.IsValid(request.Password, user!.PasswordHash, user.PasswordSalt);
        if (!pwdValid)
        {
            throw new AccounteeUnauthorizedException(ResourceRetriever.Get(CultureInfo.CurrentCulture, 
                nameof(Resources.InvalidPassword), user.Login));
        }

        var token = _jwtService.GetJwtToken(user, cancellationToken);
        
        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return token;
    }
}