using Backend.Data;
using Backend.Entities.Pitch.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Pitch
{
    public class PitchRepository : IPitchRepository
    {
        private readonly ApplicationDbContext _context;

        public PitchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PitchModel>> GetAllAsync()
        {
            return await _context.Pitches.Include(p => p.PitchType).ToListAsync();
        }

        public async Task<PitchModel> GetByIdAsync(int id)
        {
            return await _context.Pitches.Include(p => p.PitchType)
                                         .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(PitchModel pitch)
        {
            await _context.Pitches.AddAsync(pitch);
        }

        public void Update(PitchModel pitch)
        {
            _context.Pitches.Update(pitch);
        }

        public void Delete(PitchModel pitch)
        {
            _context.Pitches.Remove(pitch);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pitches.AnyAsync(p => p.Id == id);
        }
    }
}
