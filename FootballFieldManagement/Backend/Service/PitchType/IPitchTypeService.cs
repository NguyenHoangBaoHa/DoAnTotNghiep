using Backend.Entities.PitchType.Dto;

namespace Backend.Service.PitchType
{
    public interface IPitchTypeService
    {
        Task<IEnumerable<PitchTypeDto>> GetAllPitchTypesAsync();
        Task<PitchTypeDto> GetPitchTypeByIdAsync(int id);
        Task AddAsync(PitchTypeDto pitchTypeDto);
        Task UpdateAsync(int id, PitchTypeDto pitchTypeDto);
        Task DeleteAsync(int id);
    }
}
