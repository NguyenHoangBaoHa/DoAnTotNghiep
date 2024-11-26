using Backend.Entities.PitchType.Dto;

namespace Backend.Service.PitchType
{
    public interface IPitchTypesService
    {
        Task<IEnumerable<PitchTypeDto>> GetAllPitchTypesAsync();
        Task<PitchTypeDto> GetPitchTypeByIdAsync(int id);
        Task<int> AddAsync(PitchTypeDto pitchTypeDto);
        Task UpdateAsync(int id, PitchTypeDto pitchTypeDto);
        Task DeleteAsync(int id);
    }
}
