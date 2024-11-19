﻿using Backend.Data;
using Backend.Repository.Account;
using Backend.Repository.Pitch;
using Backend.Repository.PitchType;
using Backend.Repository.Staff;

namespace Backend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Accounts = new AccountRepository(_context);
            Staffs = new StaffRepository(_context);
            Pitches = new PitchRepository(_context);
            PitchesType = new PitchTypeRepository(_context);
        }

        public IAccountRepository Accounts { get; private set; }

        public IStaffRepository Staffs { get; private set; }

        public IPitchRepository Pitches { get; private set; }

        public IPitchTypeRepository PitchesType { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
