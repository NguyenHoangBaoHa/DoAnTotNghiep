using QuanLySanBong.Entities.Account.Model;

namespace QuanLySanBong.Token
{
    public interface ITokenService
    {
        string GenerateToken(AccountModel account);
    }
}
