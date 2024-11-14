﻿using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Entities.Account.Model;

namespace Backend.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AccountModel> GetById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<AccountModel> GetByEmail(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task Add(AccountModel account)
        {
            await _context.Accounts.AddAsync(account);
        }
    }
}
