using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccounteeCommon.Enums;
using AccounteeCommon.Options;
using AccounteeDomain.Entities;
using AccounteeService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AccounteeService.Services
{
    public sealed class JwtService : IJwtService
    {
        public JwtOptions JwtOptions { get; set; }

        public JwtService(JwtOptions jwtOptions)
        {
            JwtOptions = jwtOptions;
        }
    
        public string GetJwtToken(UserEntity user, CancellationToken cancellationToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new (ClaimNames.UserId, user.Id.ToString()),
                new (ClaimNames.RoleId, user.IdRole.ToString()),
                new (ClaimNames.CompanyId, user.IdCompany.ToString() ?? string.Empty),
            };
        
            var token = new JwtSecurityToken(
                JwtOptions.Issuer,    
                JwtOptions.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(JwtOptions.TokenValidMinutes),    
                signingCredentials: credentials);    
    
            var tokenStr =  new JwtSecurityTokenHandler().WriteToken(token); 
        
            return tokenStr;    
        }
    }
}