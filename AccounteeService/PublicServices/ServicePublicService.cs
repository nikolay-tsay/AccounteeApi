using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.HttpContexts;
using AccounteeDomain.Contexts;
using AccounteeDomain.Entities;
using AccounteeDomain.Entities.Enums;
using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;
using AccounteeService.Extensions;
using AccounteeService.PrivateServices.Interfaces;
using AccounteeService.PublicServices.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccounteeService.PublicServices;

public class ServicePublicService : IServicePublicService
{
    private ICurrentUserPrivateService CurrentUserPrivateService { get; }
    private AccounteeContext AccounteeContext { get; }
    private IMapper Mapper { get; }
    public ServicePublicService(ICurrentUserPrivateService currentUserPrivateService, AccounteeContext accounteeContext, IMapper mapper)
    {
        CurrentUserPrivateService = currentUserPrivateService;
        AccounteeContext = accounteeContext;
        Mapper = mapper;
    }
    public async Task<PagedList<ServiceDto>> GetServices(OrderFilter orderFilter, PageFilter pageFilter,
        CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadServices, cancellationToken);

        var services = await AccounteeContext.Services
            .AsNoTracking()
            .FilterOrder(orderFilter)
            .ToPagedList(pageFilter, cancellationToken);

        var mapped = Mapper.Map<PagedList<ServiceEntity>, PagedList<ServiceDto>>(services);

        return mapped;
    }

    public async Task<ServiceDto> GetServiceById(int serviceId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanReadServices, cancellationToken);

        var service = await AccounteeContext.Services
            .AsNoTracking()
            .Where(x => x.Id == serviceId)
            .FirstOrNotFound(cancellationToken);

        var mapped = Mapper.Map<ServiceDto>(service);

        return mapped;
    }

    public async Task<ServiceDto> CreateService(ServiceDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanCreateServices, cancellationToken);

        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new AccounteeException();
        }

        var newService = Mapper.Map<ServiceEntity>(model);

        if (newService == null)
        {
            throw new AccounteeException();
        }
        
        newService.IdCompany = GlobalHttpContext.GetCompanyId();

        AccounteeContext.Services.Add(newService);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ServiceDto>(newService);

        return mapped;
    }

    public async Task<ServiceDto> EditService(int serviceId, ServiceDto model, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditServices, cancellationToken);

        var service = await AccounteeContext.Services
            .Where(x => x.Id == serviceId)
            .FirstOrNotFound(cancellationToken);

        service.Name = model.Name ?? service.Name;
        service.Description = model.Description ?? service.Description;
        service.TotalPrice = model.TotalPrice ?? service.TotalPrice;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ServiceDto>(service);

        return mapped;
    }

    public async Task<ServiceDto> ChangeServiceCategory(int serviceId, int categoryId,
        CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanEditServices, cancellationToken);
        
        var service = await AccounteeContext.Services
            .Where(x => x.Id == serviceId)
            .FirstOrNotFound(cancellationToken);

        var category = await AccounteeContext.Categories
            .Where(x => x.Target == CategoryTargets.Service)
            .Where(x => x.Id == categoryId)
            .FirstOrNotFound(cancellationToken);

        service.IdCategory = category.Id;

        await AccounteeContext.SaveChangesAsync(cancellationToken);

        var mapped = Mapper.Map<ServiceDto>(service);

        return mapped;
    }

    public async Task<bool> DeleteService(int serviceId, CancellationToken cancellationToken)
    {
        await CurrentUserPrivateService.CheckCurrentUserRights(UserRights.CanDeleteServices, cancellationToken);

        var service = await AccounteeContext.Services
            .Where(x => x.Id == serviceId)
            .FirstOrNotFound(cancellationToken);

        AccounteeContext.Services.Remove(service);
        await AccounteeContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}