using AccounteeDomain.Models;
using AccounteeService.Contracts;
using AccounteeService.Contracts.Filters;

namespace AccounteeService.PublicServices.Interfaces;

public interface IServicePublicService
{
    Task<PagedList<ServiceDto>> GetServices(string? searchValue, OrderFilter orderFilter, PageFilter pageFilter, CancellationToken cancellationToken);
    Task<ServiceDto> GetServiceById(int serviceId, CancellationToken cancellationToken);
    Task<ServiceDto> CreateService(ServiceDto model, CancellationToken cancellationToken);
    Task<ServiceDto> EditService(int serviceId, ServiceDto model, CancellationToken cancellationToken);
    Task<ServiceDto> ChangeServiceCategory(int serviceId, int categoryId, CancellationToken cancellationToken);
    Task<bool> DeleteService(int serviceId, CancellationToken cancellationToken);
}