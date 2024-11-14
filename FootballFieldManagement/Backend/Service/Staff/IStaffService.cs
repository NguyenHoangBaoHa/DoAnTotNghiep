using Backend.Entities.Staff.Dto;

namespace Backend.Service.Staff
{
    public interface IStaffService
    {
        Task<StaffDto> GetPersonalInfo(int accountId);
        Task<bool> UpdatePersonalInfo(int accountId, StaffDto staffDto);
    }
}
