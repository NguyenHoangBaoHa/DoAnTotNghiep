using Backend.Entities.Pitch.Model;

namespace Backend.Repository.Pitch
{
    public interface IPitchRepository
    {
        Task<IEnumerable<PitchModel>> GetAllAsync();
        Task<PitchModel> GetByIdAsync(int id);
        Task AddAsync(PitchModel pitch);
        Task UpdateAsync(PitchModel pitch);
        Task DeleteAsync(PitchModel pitch);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<PitchModel>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
