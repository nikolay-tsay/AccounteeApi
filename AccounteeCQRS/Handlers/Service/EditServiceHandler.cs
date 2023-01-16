using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class EditServiceHandler : IRequestHandler<EditServiceCommand, ServiceResponse>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditServiceHandler(IServiceRepository serviceRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ServiceResponse> Handle(EditServiceCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditServices, cancellationToken);

        var service = await _serviceRepository.GetById(request.Id, true, false, cancellationToken);

        service!.Name = request.Name ?? service.Name;
        service.Description = request.Description ?? service.Description;
        service.TotalPrice = request.TotalPrice ?? service.TotalPrice;

        await _serviceRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<ServiceResponse>(service);
        return mapped;
    }
}