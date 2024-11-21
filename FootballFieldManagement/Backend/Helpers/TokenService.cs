using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Entities.Account.Model;

namespace Backend.Helpers;

public class TokenService : ITokenService
{
    //public string CreateToken(AccountModel acc, IConfiguration config)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //    var claims = new[] {
    //            new Claim(JwtRegisteredClaimNames.Name, acc.Id.ToString()),
    //            new Claim(JwtRegisteredClaimNames.UniqueName, acc.Email ?? string.Empty),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Typ, acc.Role ?? string.Empty),
    //        };

    //    var token = new JwtSecurityToken(config["Jwt:Issuer"],
    //        config["Jwt:Issuer"],
    //        claims,
    //        expires: DateTime.Now.AddDays(30),
    //        signingCredentials: credentials);

    //    return tokenHandler.WriteToken(token);
    //}

    // Hàm tạo token
    public string CreateToken(AccountModel account, IConfiguration config)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, account.Email),
            new Claim(ClaimTypes.Role, account.Role) // Lấy vai trò của người dùng
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}