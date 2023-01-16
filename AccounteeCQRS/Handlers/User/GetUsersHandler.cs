using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.User;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.User;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedList<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUsersHandler(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<PagedList<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadUsers, cancellationToken);
        
        var users = await _userRepository
            .QueryAll(false)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedList(request.PageFilter, cancellationToken);
        
        var mapped = _mapper.Map<PagedList<UserResponse>>(users);
        return mapped;
    }
}