using Backend.Entities.Account.Model;

namespace Backend.Helpers;

public interface ITokenService
{
    string CreateToken(AccountModel acc, IConfiguration config);
}