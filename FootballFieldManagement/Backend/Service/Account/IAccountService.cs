using Backend.Entities.Account.Dto;
using Backend.Entities.Account.Model;

namespace Backend.Service.Account
{
    public interface IAccountService
    {
        Task<LoginModel> Login(LoginDto loginDto);
        Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto);
    }
}
