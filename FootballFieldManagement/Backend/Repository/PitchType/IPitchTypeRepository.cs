using Backend.Entities.PitchType.Model;

namespace Backend.Repository.PitchType
{
    public interface IPitchTypeRepository
    {
        Task<IEnumerable<PitchTypeModel>> GetAllPitchTypesAsync();
        Task<PitchTypeModel> GetPitchTypeByIdAsync(int id);
        Task AddAsync(PitchTypeModel pitchType);
        Task UpdateAsync(PitchTypeModel pitchType);
        Task DeleteAsync(int id);
    }
}
