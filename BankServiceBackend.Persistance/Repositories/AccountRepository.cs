using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;
using BankServiceBackend.Persistance.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Persistance.Repositories
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
            try
            {
                return await _context.Accounts.ToListAsync();
            }
            catch (Exception)
            {
                throw new FetchingFailedException("Fetching accounts failed!");
            }
        }

        public async Task<Account> GetAsync(long accountNumber)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);
            if (account == null)
            {
                throw new FetchingFailedException($"Account number {account} is unknown!");
            }

            return account;
        }

        public async Task<Account> UpdateAsync(long accountNumber, Account account)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                account.AccountNumber = accountNumber;
                _context.Update(account);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return account;
            }
            catch (InvalidOperationException)
            {
                throw new PersistingFailedException($"Account number {accountNumber} is unknown. Updating {account} failed!");
            }
            catch (Exception)
            {
                throw new PersistingFailedException($"Updating {account} failed!");
            }
        }

        public async Task<Account> SaveAsync(Account account)
        {
            try
            {
                account =_context.Accounts.Add(account).Entity;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                throw new PersistingFailedException($"Saving {account} failed!");
            }

            return account;
        }

        public async Task<Account> DeleteAsync(long accountNumber)
        {
            try
            {
                var account = await GetAsync(accountNumber);
                if (account != null)
                {
                    await _context.Database.BeginTransactionAsync();

                    if (Math.Abs(account.Credit) > 0.0001)
                    {
                        throw new RemovingFailedException("Account has credit. Removing is not possible!");
                    }

                    // Remove account from users
                    foreach (var user in account.Users)
                    {
                        user.Accounts.Remove(account);
                    }

                    _context.Accounts.Remove(account);

                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }

                return account;
            }
            catch (Exception)
            {
                throw new RemovingFailedException($"Removing user with customer number = {accountNumber} failed!");
            }
        }
    }
}
