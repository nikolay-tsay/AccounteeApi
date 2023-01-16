using AccounteeCommon.Enums;
using AccounteeCQRS.Requests.Service;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Service;

public sealed class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, bool>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteServiceHandler(IServiceRepository serviceRepository, ICurrentUserService currentUserService)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        await _currentUserService.CheckCurrentUserRights(UserRights.CanDeleteServices, cancellationToken);

        var service = await _serviceRepository.GetById(request.Id, true, false, cancellationToken);
        await _serviceRepository.DeleteService(service!, true, cancellationToken);

        return true;
    }
}