using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AccounteeCommon.Options;
using AccounteeDomain.Entities;
using AccounteeService.PrivateServices.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AccounteeService.PrivateServices;

public class AuthPrivateService : IAuthPrivateService
{
    public JwtOptions JwtOptions { get; set; }

    public AuthPrivateService(JwtOptions jwtOptions)
    {
        JwtOptions = jwtOptions;
    }
    
    public string GetJwtToken(UserEntity user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var token = new JwtSecurityToken(
            JwtOptions.Issuer,    
            JwtOptions.Audience,
            expires: DateTime.Now.AddMinutes(JwtOptions.TokenValidMinutes),    
            signingCredentials: credentials);    
    
        var tokenStr =  new JwtSecurityTokenHandler().WriteToken(token); 
        
        return tokenStr;    
    }
}