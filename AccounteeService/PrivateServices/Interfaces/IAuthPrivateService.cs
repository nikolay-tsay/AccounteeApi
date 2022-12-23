using AccounteeDomain.Entities;

namespace AccounteeService.PrivateServices.Interfaces;

public interface IAuthPrivateService
{
    string GetJwtToken(UserEntity user, CancellationToken cancellationToken);
}