using AccounteeDomain.Entities;

namespace AccounteeService.Services.Interfaces;

public interface IJwtService
{
    string GetJwtToken(UserEntity user, CancellationToken cancellationToken);
}