using Backend.Entities.Account.Dto;
using Backend.Entities.Account.Model;
using Backend.Entities.Customer.Dto;

namespace Backend.Service.Account
{
    public interface IAccountService
    {
        Task<LoginModel> Login(LoginDto loginDto);
        Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto);
        Task<AccountModel> RegisterCustomer(CustomerDto customerDto, AccountDto accountDto);
    }
}
