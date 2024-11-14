using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Entities.Account.Model;

namespace Backend.Helpers;

public class TokenService : ITokenService
{
    public string CreateToken(AccountModel acc, IConfiguration config)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Name, acc.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, acc.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, acc.Role ?? string.Empty),
            };

        var token = new JwtSecurityToken(config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: credentials);

        return tokenHandler.WriteToken(token);
    }
}