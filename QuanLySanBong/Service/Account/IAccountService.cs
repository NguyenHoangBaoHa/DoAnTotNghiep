using QuanLySanBong.Entities.Account.Dto;
using QuanLySanBong.Entities.Account.Model;

namespace QuanLySanBong.Service.Account
{
    public interface IAccountService
    {
        Task<LoginModel> Login(LoginDto loginDto);
        Task<AccountModel> CreateStaff(CreateStaffDto createStaffDto);
    }
}
