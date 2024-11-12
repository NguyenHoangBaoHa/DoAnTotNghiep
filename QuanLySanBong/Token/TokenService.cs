using Microsoft.IdentityModel.Tokens;
using QuanLySanBong.Entities.Account.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLySanBong.Token
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationInDays;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _expirationInDays = int.Parse(configuration["JwtSettings:ExpiryInDay"]);
        }

        public string GenerateToken(AccountModel account)
        {
            var claims = new[]
            {
        new Claim("Id", account.Id.ToString()),
        new Claim(ClaimTypes.Email, account.Email),
        new Claim(ClaimTypes.Role, account.Role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.Now.AddDays(_expirationInDays); // Thay đổi ở đây
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
