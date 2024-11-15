using Backend.Entities.Account.Model;
using Backend.Entities.Customer.Model;

namespace Backend.Repository.Account
{
    public interface IAccountRepository
    {
        Task<AccountModel> GetById(int id);
        Task<AccountModel> GetByEmail(string email);
        Task Add(AccountModel account);
        Task AddCustomer(CustomerModel customer);
    }
}
