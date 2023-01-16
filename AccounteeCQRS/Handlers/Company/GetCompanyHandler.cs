using AccounteeCommon.Exceptions;
using AccounteeCommon.Resources;
using AccounteeCQRS.Requests.Company;
using AccounteeCQRS.Responses;
using AccounteeService.Services.Interfaces;
using AutoMapper;
using MediatR;

namespace AccounteeCQRS.Handlers.Company;

public class GetCompanyHandler : IRequestHandler<GetCompanyQuery, CompanyResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCompanyHandler(ICurrentUserService currentUserService, IMapper mapper)
    {
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<CompanyResponse> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUser(false, cancellationToken);
        if (currentUser.User.Company is null)
        {
            throw new AccounteeException(ResourceRetriever.Get(currentUser.Culture,
                nameof(Resources.UserNoCompany), currentUser.User.Login));
        }

        var mapped = _mapper.Map<CompanyResponse>(currentUser.User.Company);
        return mapped;
    }
}