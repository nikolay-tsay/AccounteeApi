using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponse>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetServiceByIdHandler(IServiceRepository serviceRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ServiceResponse> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanReadServices, cancellationToken);

        var service = await _serviceRepository.GetById(request.Id, false, false, cancellationToken);

        var mapped = _mapper.Map<ServiceResponse>(service);
        return mapped;
    }
}