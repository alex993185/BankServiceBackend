using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankService.Backend.Persistance.Entities;
using BankService.Backend.Persistance.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Persistance.Repositories
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
                return await _context.Accounts.Include(a => a.Users).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw new FetchingFailedException("Fetching accounts failed!");
            }
        }

        public async Task<Account> GetAsync(long accountNumber)
        {
            try
            {
                var account = await _context.Accounts.Include(a => a.Users).AsNoTracking().FirstAsync(a => a.AccountNumber == accountNumber);
                return account;
            }
            catch (Exception)
            {
                throw new FetchingFailedException($"Account number {accountNumber} is unknown!");
            }
          
        }

        public async Task<Account> UpdateAsync(long accountNumber, string hashedPin, Account account)
        {
            try
            {
                var persistedAccount = await _context.Accounts.FindAsync(accountNumber);
                if (persistedAccount == null)
                {
                    throw new PersistingFailedException(
                        $"Account number {accountNumber} is unknown. Updating {account} failed!");
                }

                if (persistedAccount.HashedPin != hashedPin)
                {
                    throw new PersistingFailedException("Wrong PIN!");
                }

                persistedAccount.Dispo = account.Dispo;
                persistedAccount.Name = account.Name;
                await _context.SaveChangesAsync();
                _context.Entry(persistedAccount).State = EntityState.Detached;
                return persistedAccount;
            }
            catch (PersistingFailedException e)
            {
                throw e;
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

                account.AccountNumber = 0;
                account.Credit = 0;
                account.HashedPin = hashedPin;
                account = _context.Accounts.Add(account).Entity;
                await _context.SaveChangesAsync();
                _context.Entry(account).State = EntityState.Detached;
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
                var account = await _context.Accounts.Include(a => a.Users).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                if (account != null)
                {
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
                    _context.Entry(account).State = EntityState.Detached;
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

        public async Task Assign(long accountNumber, long customerNumber)
        {
            var account = await _context.Accounts.Include(a => a.Users).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            var user = await _context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.CustomerNumber == customerNumber);
            if (account == null || user == null)
            {
                throw new UserFriendlyException("Account or user not found!");
            }

            if (account.Users.All(u => u.CustomerNumber != user.CustomerNumber))
            {
                account.Users.Add(user);
                user.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException("Account already assigned to the user!");
            }
        }

        public async Task DepositAsync(long accountNumber, double amountInEuro)
        {
            var account = await _context.Accounts.Include(a => a.Users).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                throw new UserFriendlyException("Account not found!");
            }

            account.Credit += amountInEuro;
            await _context.SaveChangesAsync();
        }

        public async Task WithdrawAsync(long accountNumber, double amountInEuro)
        {
            var account = await _context.Accounts.Include(a => a.Users).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                throw new UserFriendlyException("Account not found!");
            }

            account.Credit -= amountInEuro;
            await _context.SaveChangesAsync();
        }
    }
}