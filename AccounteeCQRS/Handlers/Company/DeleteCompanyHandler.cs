using AccounteeCommon.Enums;
using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Category;
using AccounteeService.Repositories.Interfaces;
using AccounteeService.Services.Interfaces;
using MediatR;

namespace AccounteeCQRS.Handlers.Company;

public sealed class DeleteCompanyHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCompanyHandler(ICompanyRepository companyRepository, ICurrentUserService currentUserService)
    {
        _companyRepository = companyRepository;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(true, cancellationToken);
        _currentUserService.CheckUserRights(currentUser.User, UserRights.CanDeleteCompany);
        
        if (currentUser.User.Company is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture, 
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        currentUser.User.IdRole = 1;
        await _companyRepository.DeleteCompany(currentUser.User.Company, true, cancellationToken);
        
        return true;
    }
}