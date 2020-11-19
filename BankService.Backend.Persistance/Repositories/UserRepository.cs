using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankService.Backend.Persistance.Entities;
using BankService.Backend.Persistance.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankService.Backend.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BankServiceDbContext _context;

        public UserRepository(BankServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.Include(u => u.Accounts).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw new FetchingFailedException("Fetching users failed!");
            }

        }

        public async Task<User> GetAsync(long customerNumber)
        {
            var user = await _context.Users.Include(u => u.Accounts).AsNoTracking().FirstOrDefaultAsync(u => u.CustomerNumber == customerNumber);
            if (user == null)
            {
                throw new FetchingFailedException($"Customer number {customerNumber} is unknown!");
            }

            _context.Entry(user).State = EntityState.Detached;
            return user;
        }

        public async Task<User> UpdateAsync(long customerNumber, User user)
        {
            try
            {
                var persistedUser = await _context.Users.FindAsync(customerNumber);
                persistedUser.Birthday = user.Birthday;
                persistedUser.Name = user.Name;
                persistedUser.FirstName = user.FirstName;
                persistedUser.Gender = user.Gender;
                await _context.SaveChangesAsync();
                _context.Entry(persistedUser).State = EntityState.Detached;
                return persistedUser;
            }
            catch (InvalidOperationException)
            {
                throw new PersistingFailedException($"Customer number {customerNumber} is unknown. Updating {user} failed!");
            }
            catch (Exception)
            {
                throw new PersistingFailedException($"Updating {user} failed!");
            }
        }

        public async Task<User> SaveAsync(User user)
        {
            try
            {
                user = _context.Users.Add(user).Entity;
                await _context.SaveChangesAsync();
                _context.Entry(user).State = EntityState.Detached;
            }
            catch (Exception)
            {
                throw  new PersistingFailedException($"Saving {user} failed!");
            }

            return user;
        }

        public async Task<User> RemoveAsync(long customerNumber)
        {
            try
            {
                var user = await GetAsync(customerNumber);
                if (user != null)
                {
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
                            return null;
                        }
                    }

                    _context.Users.Remove(user);

                    await _context.SaveChangesAsync();
                    _context.Entry(user).State = EntityState.Detached;
                }

                return user;
            }
            catch (Exception)
            {
                throw new RemovingFailedException($"Removing user with customer number = {customerNumber} failed!");
            }
        }
    }
}