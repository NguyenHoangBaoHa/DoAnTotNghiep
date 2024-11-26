using QuanLySanBong.Repository.Account;
using QuanLySanBong.Repository.Staff;

namespace QuanLySanBong.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        IStaffRepository Staffs { get; }

        Task<int> CompleteAsync();
    }
}
