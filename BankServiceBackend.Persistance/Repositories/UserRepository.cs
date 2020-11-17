using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankServiceBackend.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankServiceBackend.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgresDbContext _context;

        public UserRepository(PostgresDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetAsync(long customerNumber)
        {
            return await _context.Users.FindAsync(customerNumber);
        }

        public async Task<User> UpdateAsync(long customerNumber, User user)
        {
            await _context.Database.BeginTransactionAsync();
            user.CustomerNumber = customerNumber;
            _context.Update(user);
            try
            {
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception($"Updating {user} failed!");
            }
        }

        public async Task<User> SaveAsync(User user)
        {
            await _context.Database.BeginTransactionAsync();
            user = _context.Users.Add(user).Entity;
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
            return user;
        }

        public async Task<User> RemoveAsync(long customerNumber)
        {
            var user = await GetAsync(customerNumber);
            if (user != null)
            {
                await _context.Database.BeginTransactionAsync();

                // Handle accounts that are linked to user
                foreach (var userAccount in user.Accounts)
                {
                    if (userAccount.Users.Any(u => u.CustomerNumber != user.CustomerNumber))
                    {
                        userAccount.Users.Remove(user);
                    }
                    else
                    {
                        // User can not be deleted because he is linked to an account no other user is linked to
                        await _context.Database.RollbackTransactionAsync();
                        return null;
                    }
                }
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }

            return user;
        }
    }
}
