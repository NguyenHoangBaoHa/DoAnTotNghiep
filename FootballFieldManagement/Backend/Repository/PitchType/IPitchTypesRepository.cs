using Backend.Entities.PitchType.Model;

namespace Backend.Repository.PitchType
{
    public interface IPitchTypesRepository
    {
        Task<IEnumerable<PitchTypeModel>> GetAllPitchTypesAsync();
        Task<PitchTypeModel> GetPitchTypeByIdAsync(int id);
        Task<PitchTypeModel> GetPitchTypeByNameAsync(string name);
        Task AddAsync(PitchTypeModel pitchType);
        Task UpdateAsync(PitchTypeModel pitchType);
        Task DeleteAsync(int id);
    }
}
