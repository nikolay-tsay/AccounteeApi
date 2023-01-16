using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Service;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities.Enums;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class ChangeServiceCategoryHandler : IRequestHandler<ChangeServiceCategoryCommand, ServiceResponse>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ChangeServiceCategoryHandler(IServiceRepository serviceRepository, ICategoryRepository categoryRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<ServiceResponse> Handle(ChangeServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanEditServices, cancellationToken);
        
        var service = await _serviceRepository.GetById(request.ServiceId, true, false, cancellationToken);
        var category = await _categoryRepository.GetById(request.CategoryId, CategoryTargets.Service, true, false, cancellationToken);

        service!.IdCategory = category!.Id;

        await _serviceRepository.SaveChanges(cancellationToken);

        var mapped = _mapper.Map<ServiceResponse>(service);
        return mapped;
    }
}