﻿using Backend.Data;
using Backend.Entities.PitchType.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.PitchType
{
    public class PitchTypeRepository : IPitchTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PitchTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PitchTypeModel>> GetAllPitchTypesAsync()
        {
            return await _context.PitchesType.ToListAsync();
        }

        public async Task<PitchTypeModel> GetPitchTypeByIdAsync(int id)
        {
            return await _context.PitchesType.FindAsync(id);
        }

        public async Task AddAsync(PitchTypeModel pitchType)
        {
            await _context.PitchesType.AddAsync(pitchType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PitchTypeModel pitchType)
        {
            _context.PitchesType.Update(pitchType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pitchType = await _context.PitchesType.FindAsync(id);
            if (pitchType != null)
            {
                _context.PitchesType.Remove(pitchType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
