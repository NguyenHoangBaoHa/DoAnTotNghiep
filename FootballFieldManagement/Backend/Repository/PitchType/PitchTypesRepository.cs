﻿using Backend.Data;
using Backend.Entities.PitchType.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.PitchType
{
    public class PitchTypesRepository : IPitchTypesRepository
    {
        private readonly ApplicationDbContext _context;

        public PitchTypesRepository(ApplicationDbContext context)
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

        public async Task<PitchTypeModel> GetPitchTypeByNameAsync(string name)
        {
            return await _context.PitchesType
                .FirstOrDefaultAsync(pt => pt.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(PitchTypeModel pitchType)
        {
            try
            {
                await _context.PitchesType.AddAsync(pitchType);
                await _context.SaveChangesAsync(); // Lưu thay đổi vào database
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, có thể log chi tiết hoặc thông báo lỗi cho người dùng
                throw new Exception("An error occurred while saving the pitch type to the database.", ex);
            }
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
