using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class EditUserHandler : IRequestHandler<EditUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<UserResponse> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditUsers, cancellationToken);

        var user = await _userRepository.GetById(request.Id, true, false, cancellationToken);

        user!.Login = request.Login ?? user.Login;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.IncomePercent = request.IncomePercent ?? user.IncomePercent;

        await _userRepository.SaveChanges(cancellationToken);
        
        var mapped = _mapper.Map<UserResponse>(user);
        return mapped;
    }
}