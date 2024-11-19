using Backend.Entities.Pitch.Model;

namespace Backend.Repository.Pitch
{
    public interface IPitchRepository
    {
        Task<IEnumerable<PitchModel>> GetAllAsync();
        Task<PitchModel> GetByIdAsync(int id);
        Task AddAsync(PitchModel pitch);
        void Update(PitchModel pitch);
        void Delete(PitchModel pitch);
        Task<bool> ExistsAsync(int id);
    }
}
