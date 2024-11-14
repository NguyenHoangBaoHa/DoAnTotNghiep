using Backend.Entities.Staff.Model;

namespace Backend.Repository.Staff
{
    public interface IStaffRepository
    {
        Task<StaffModel> GetStaffByAccountId(int accountId);
        Task Add(StaffModel staff);
        Task Update(StaffModel staff);
    }
}
