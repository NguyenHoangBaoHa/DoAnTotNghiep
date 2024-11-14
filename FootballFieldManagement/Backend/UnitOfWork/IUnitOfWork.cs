using Backend.Repository.Account;
using Backend.Repository.Staff;

namespace Backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        IStaffRepository Staffs { get; }

        Task<int> CompleteAsync();
    }
}
