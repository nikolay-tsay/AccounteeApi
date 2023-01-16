using System.Globalization;
using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHandler _passwordHandler;
    private readonly IMapper _mapper;

    public RegisterUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper, IPasswordHandler passwordHandler)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _passwordHandler = passwordHandler;
    }
    
    public async Task<UserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanRegisterUsers);
        
        var existing = await _userRepository.GetByLogin(request.Login, false, true, cancellationToken);
        if (existing is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(CultureInfo.CurrentCulture,
                nameof(Resources.AlreadyExists), nameof(UserEntity)));
        }
        
        _passwordHandler.CreateHash(request.Password, out string hash, out string salt);
        var newUser = new UserEntity
        {
            IdCompany = currentUser.User.IdCompany,
            IdRole = request.IdRole,
            Login = request.Login,
            UserLanguage = request.Language,
            PasswordHash = hash,
            PasswordSalt = salt,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IncomePercent = request.IncomePercent
        };

        await _userRepository.AddUser(newUser, true, cancellationToken);
        
        var mapped = _mapper.Map<UserResponse>(newUser);
        return mapped;
    }
}