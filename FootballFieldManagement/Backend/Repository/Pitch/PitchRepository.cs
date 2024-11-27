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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PitchModel pitch)
        {
            _context.Pitches.Update(pitch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PitchModel pitch)
        {
            _context.Pitches.Remove(pitch);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pitches.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PitchModel>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Pitches
                                 .Include(p => p.PitchType)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }
    }
}
