using System.Globalization;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Auth;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Auth;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHandler _passwordHandler;
    private readonly IMapper _mapper;

    public RegisterHandler(IUserRepository userRepository, IPasswordHandler passwordHandler, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHandler = passwordHandler;
        _mapper = mapper;
    }
    
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);

        var existingUser = await _userRepository.GetByLogin(request.Login, false, true, cancellationToken);
        if (existingUser is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(CultureInfo.CurrentCulture,
                nameof(Resources.AlreadyExists), nameof(UserEntity)));
        }
        
        _passwordHandler.CreateHash(request.Password, out string hash, out string salt);
        
        var newUser = new UserEntity
        {
            IdRole = 1,
            Login = request.Login,
            UserLanguage = request.Language,
            PasswordHash = hash,
            PasswordSalt = salt,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _userRepository.AddUser(newUser, true, cancellationToken);
        
        var mapped = _mapper.Map<RegisterResponse>(newUser);
        
        GlobalHttpContext.SetIgnoreCompanyFilter(false);
        return mapped;
    }
}