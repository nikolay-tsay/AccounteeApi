using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Company;
using AccounteeCQRS.Responses;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Company;

public sealed class EditCompanyHandler : IRequestHandler<EditCompanyCommand, CompanyResponse>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public EditCompanyHandler(ICompanyRepository companyRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CompanyResponse> Handle(EditCompanyCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(true, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanEditCompany);
        
        if (currentUser.User.Company is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        currentUser.User.Company.Email = request.Email ?? currentUser.User.Company.Email;
        currentUser.User.Company.PhoneNumber = request.PhoneNumber ?? currentUser.User.Company.PhoneNumber;
        currentUser.User.Company.Name = request.Name ?? currentUser.User.Company.Name;
        currentUser.User.Company.Budget = request.Budget ?? currentUser.User.Company.Budget;

        await _companyRepository.SaveChanges(cancellationToken);
        var mapped = _mapper.Map<CompanyResponse>(currentUser.User.Company);
        
        return mapped;
    }
}