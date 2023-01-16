using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class CreateServiceHandler : IRequestHandler<CreateServiceCommand, ServiceResponse>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateServiceHandler(IServiceRepository serviceRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ServiceResponse> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanCreateProducts);

        var existing = await _serviceRepository.GetByName(request.Name, false, true, cancellationToken);
        if (existing is not null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.AlreadyExists), nameof(ServiceEntity)));
        }
        
        var newService = _mapper.Map<ServiceEntity>(request);
        if (newService is null)
        { 
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.MappingError), new object[] {nameof(CreateServiceCommand), nameof(ServiceEntity)}));
        }
        
        newService.IdCompany = currentUser.User.IdCompany;
        await _serviceRepository.AddService(newService, true, cancellationToken);

        var mapped = _mapper.Map<ServiceResponse>(newService);
        return mapped;
    }
}