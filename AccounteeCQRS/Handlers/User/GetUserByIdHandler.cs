using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadUsers, cancellationToken);

        var user = await _userRepository.GetById(request.Id, false, false, cancellationToken);

        var mapped = _mapper.Map<UserResponse>(user);
        return mapped;
    }
}