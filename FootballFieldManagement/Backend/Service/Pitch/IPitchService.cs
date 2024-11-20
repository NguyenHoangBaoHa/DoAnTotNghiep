using Backend.Entities.Pitch.Dto;

namespace Backend.Service.Pitch
{
    public interface IPitchService
    {
        Task<IEnumerable<PitchDto>> GetAllPitches();
        Task<PitchDto> GetPitchById(int id);
        Task<PitchDto> CreatePitch(PitchDto pitchDto);
        Task<PitchDto> UpdatePitch(int id, PitchDto pitchDto);
        Task DeletePitch(int id);
        Task<bool> CheckPitchTypeExists(int? IdPitchType);
    }
}
