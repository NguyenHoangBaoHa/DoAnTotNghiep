using QuanLySanBong.Entities.Account.Model;

namespace QuanLySanBong.Repository.Account
{
    public interface IAccountRepository
    {
        Task<AccountModel> GetById(int id);
        Task<AccountModel> GetByEmail(string email);
        Task Add(AccountModel account);
    }
}
