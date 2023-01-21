using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeService.Contracts;
using AccounteeService.Extensions;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class GetServicesHandler : IRequestHandler<GetServicesQuery, PagedList<ServiceResponse>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetServicesHandler(IServiceRepository serviceRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<PagedList<ServiceResponse>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadServices, cancellationToken);

        var services = await _serviceRepository
            .QueryAll(false)
            .ApplySearch(request.SearchValue)
            .FilterOrder(request.OrderFilter)
            .ToPagedListAsync(request.PageFilter, cancellationToken);
        
        var mapped = _mapper.Map<PagedList<ServiceResponse>>(services);
        return mapped;
    }
}