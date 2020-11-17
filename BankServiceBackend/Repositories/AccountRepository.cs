﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Database;
using BankServiceBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly PostgresDbContext _context;

        public AccountRepository(PostgresDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> Get(long accountNumber)
        {
            return await _context.Accounts.FindAsync(accountNumber);
        }

        public async Task<bool> Update(long accountNumber, Account account)
        {
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(accountNumber))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        private bool AccountExists(long accountNumber)
        {
            return _context.Accounts.Any(e => e.AccountNumber == accountNumber);
        }

        public async Task Save(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Account account)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
