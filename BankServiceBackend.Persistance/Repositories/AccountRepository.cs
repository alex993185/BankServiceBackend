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
        private readonly BankServiceDbContext _context;

        public AccountRepository(BankServiceDbContext context)
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
            Console.WriteLine(account == null);
            if (account == null)
            {
                throw new FetchingFailedException($"Account number {account} is unknown!");
            }

            return account;
        }

        public async Task<Account> UpdateAsync(long accountNumber, string hashedPin, Account account)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var persistedAccount = await GetAsync(accountNumber);
                if (persistedAccount.HashedPin != hashedPin)
                {
                    throw new PersistingFailedException("Wrong PIN!");
                }

                persistedAccount.Dispo = account.Dispo;
                persistedAccount.Name = account.Name;
                _context.Update(persistedAccount);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return account;
            }
            catch (PersistingFailedException e)
            {
                throw e;
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

        public async Task<Account> SaveAsync(string hashedPin, Account account)
        {
            try
            {
                if (string.IsNullOrEmpty(hashedPin))
                {
                    throw new PersistingFailedException("There must be a PIN entered!");
                }

                account.Credit = 0;
                account.HashedPin = hashedPin;
                account = _context.Accounts.Add(account).Entity;
                await _context.SaveChangesAsync();
            }
            catch (PersistingFailedException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new PersistingFailedException($"Saving {account} failed!");
            }

            return account;
        }

        public async Task<Account> DeleteAsync(long accountNumber, string hashedPin)
        {
            try
            {
                var account = await GetAsync(accountNumber);
                if (account != null)
                {
                    await _context.Database.BeginTransactionAsync();
                    if (account.HashedPin != hashedPin)
                    {
                        throw new RemovingFailedException("Wrong PIN!");
                    }
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
            catch (RemovingFailedException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new RemovingFailedException($"Removing user with customer number = {accountNumber} failed!");
            }
        }
    }
}
