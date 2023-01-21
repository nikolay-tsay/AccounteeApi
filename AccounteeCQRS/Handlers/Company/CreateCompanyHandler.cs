using AccounteeCommon.Enums;
using AccounteeCommon.HttpContexts;
using AccounteeCQRS.Requests.Company;
using AccounteeCQRS.Responses;
using AccounteeDomain.Entities;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Company;

public sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateCompanyHandler(ICompanyRepository companyRepository, IRoleRepository roleRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<CompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        GlobalHttpContext.SetIgnoreCompanyFilter(true);

        var currentUser = await _currentUserService.GetCurrentUser(true, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanCreateCompany);

        var newCompany = new CompanyEntity
        {
            Name = request.Name,
            Email = request.Email ?? currentUser.User.Email,
            PhoneNumber = request.PhoneNumber,
            Budget = request.Budget ?? 0
        };

        await _companyRepository.AddCompany(newCompany, false, cancellationToken);
        var ownerRole = new RoleEntity
        {
            Company = newCompany,
            Name = "Owner",
            IsAdmin = true
        };

        await _roleRepository.AddRole(ownerRole, false, cancellationToken);
        currentUser.User.Role = ownerRole;
        currentUser.User.Company = newCompany;

        await _companyRepository.SaveChanges(cancellationToken);
        GlobalHttpContext.SetIgnoreCompanyFilter(false);

        var mapped = _mapper.Map<CompanyResponse>(newCompany);
        return mapped;
    }
}